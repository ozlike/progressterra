using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class EventLogger : IHostedService, IDisposable
    {
        readonly IServiceScopeFactory scopeFactory;
        Thread interrogatingThread;

        public EventLogger(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
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
                InterrogateServices();
                Thread.Sleep(60 * 1000); //раз в минуту
            }
        }

        private async void InterrogateServices()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProgressterraContext>();
                InterrogationService interrogationService = scope.ServiceProvider.GetRequiredService<InterrogationService>();
                var dataSender = scope.ServiceProvider.GetRequiredService<IHubContext<DataSender>>();


                List<Event> events = await interrogationService.InterrogateServises();
                await context.Events.AddRangeAsync(events);
                await context.SaveChangesAsync();

                await dataSender.Clients.All.SendAsync("broadcastMessage", "hi my dear");

            }
        }
    }
}
