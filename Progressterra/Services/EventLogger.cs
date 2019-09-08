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
            interrogatingThread = new Thread(InterrogateServices);
            interrogatingThread.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void InterrogateServices()
        {
            while (true)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ProgressterraContext>();
                    InterrogationService interrogationService = scope.ServiceProvider.GetRequiredService<InterrogationService>();


                    List<Event> events = await interrogationService.InterrogateServises();
                    await context.Events.AddRangeAsync(events);
                    await context.SaveChangesAsync();
                }


                //раз в минуту
                Thread.Sleep(60 * 1000);
            }
        }
    }
}
