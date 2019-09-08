using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Progressterra.Context;
using Progressterra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class ServiceEventsHandler : IHostedService, IDisposable
    {
        public long MaxResponseTime { get; private set; }
        public int PollingRate { get; private set; }

        readonly IServiceScopeFactory scopeFactory;
        DateTime LastUpdate;
        Thread interrogatingThread;
        List<ServiceDayEvents> ServicesStates;

        public ServiceEventsHandler(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            interrogatingThread.Abort();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<ConfigClass>();
                MaxResponseTime = configuration.MaxResponseTime;
                PollingRate = configuration.PollingRate;

                var context = scope.ServiceProvider.GetRequiredService<ProgressterraContext>();

                if (context.Services == null || context.Services.Count() == 0) TEMP_ADD_INIT_VALUES_INTO_DATABASE(context);

                ServicesStates = context.Services.Select(x => new ServiceDayEvents
                {
                    Status = new ServiceStatus
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url,
                    },
                    Events = x.Events.Where(y => y.EventTime > DateTime.Now.AddDays(-1)).ToList(),
                }).ToList();

                foreach (var serv in ServicesStates)
                {
                    UpdateEventsInLastDay(serv, false);
                    UpdateEventsInLastHour(serv);
                }
            }
            LastUpdate = DateTime.Now;
            interrogatingThread = new Thread(Interrogating);
            interrogatingThread.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void Interrogating()
        {
            while (true)
            {
                InterrogateAndBroadcastStates();                
                Thread.Sleep(PollingRate * 1000);
            }
        }

        private async void InterrogateAndBroadcastStates()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dataSender = scope.ServiceProvider.GetRequiredService<IHubContext<DataSender>>();
                await dataSender.Clients.All.SendAsync("broadcastMessage", await InterrogateServices(true));
            }
        }

        public async Task<List<ServiceStatus>> InterrogateServices(bool writeToDatabase)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProgressterraContext>();
                var interrogation = scope.ServiceProvider.GetRequiredService<InterrogationService>();

                var events = await interrogation.InterrogateServises();
                if (writeToDatabase)
                {
                    await context.Events.AddRangeAsync(events);
                    await context.SaveChangesAsync();
                }


                bool needUpdate = LastUpdate.AddHours(1) < DateTime.Now;
                if (needUpdate) LastUpdate = DateTime.Now;
                foreach (var serv in ServicesStates)
                {
                    var currentEvent = events.FirstOrDefault(x => x.ServiceId == serv.Status.Id);
                    serv.Status.ResponseTime = currentEvent.ResponseTime;
                    serv.Status.Available = currentEvent.Available;
                    serv.Events.Add(currentEvent);

                    if (needUpdate) UpdateEventsInLastDay(serv, true);

                    if (!currentEvent.Available) serv.Status.FailsInLastDay++;
                    else
                    if (currentEvent.ResponseTime > MaxResponseTime * 2)
                    {
                        if (serv.Status.MaxDeviationInLastDay == null || currentEvent.ResponseTime > serv.Status.MaxDeviationInLastDay)
                            serv.Status.MaxDeviationInLastDay = currentEvent.ResponseTime;
                    }

                    UpdateEventsInLastHour(serv);
                }

                return ServicesStates.Select(x => x.Status).ToList();
            }
        }
        
        private void UpdateEventsInLastDay(ServiceDayEvents serviceEvents, bool clip)
        {
            if (clip) serviceEvents.Events = serviceEvents.Events.Where(x => x.EventTime > DateTime.Now.AddDays(-1)).ToList();
            serviceEvents.Status.FailsInLastDay = FailsInEvents(serviceEvents.Events);
            serviceEvents.Status.MaxDeviationInLastDay = MaxDeviationInEvents(serviceEvents.Events);
        }

        private void UpdateEventsInLastHour(ServiceDayEvents serviceEvents)
        {
            var lastHourEvents = serviceEvents.Events.Where(x => x.EventTime > DateTime.Now.AddHours(-1)).ToList();
            serviceEvents.Status.FailsInLastHour = FailsInEvents(lastHourEvents);
            serviceEvents.Status.MaxDeviationInLastHour = MaxDeviationInEvents(lastHourEvents);
        }

        private int FailsInEvents(ICollection<Event> events) => events.Count(x => !x.Available);
        private long? MaxDeviationInEvents(ICollection<Event> events) => events.Where(x => x.Available && x.ResponseTime > MaxResponseTime * 2)?.Max(x => x.ResponseTime);


        private void TEMP_ADD_INIT_VALUES_INTO_DATABASE(ProgressterraContext context)
        {
            context.Services.Add(new Service
            {
                Name = "ibonus",
                Url = "http://ibonus.1c-work.net/api/ibonus/version",
            });
            context.Services.Add(new Service
            {
                Name = "refdata",
                Url = "http://iswiftdata.1c-work.net/api/refdata/version",
            });
            context.Services.Add(new Service
            {
                Name = "catalog",
                Url = "http://iswiftdata.1c-work.net/api/catalog/catalog",
                Headers = new List<Header>
                {
                    new Header()
                    {
                        Name = "AccessKey",
                        Value = "test_05fc5ed1-0199-4259-92a0-2cd58214b29c",
                    }
                },
            });
            context.SaveChanges();
        }
    }
}