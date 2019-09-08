using Microsoft.EntityFrameworkCore;
using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class InterrogationService
    {
        ProgressterraContext context;
        public InterrogationService(ProgressterraContext context)
        {
            this.context = context;
        }

        public async Task<List<Event>> InterrogateServises()
        {
            List<Event> events = new List<Event>();

            foreach (var service in context.Services.Include(x => x.Headers))
            {
                events.Add(await MakeQuery(service));
            }

            return events;
        }

        private async Task<Event> MakeQuery(Service service)
        {
            using (var httpClient = new HttpClient())
            {
                if (service.Headers != null && service.Headers.Count != 0)
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    foreach (var header in service.Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }
                }
                
                var stopWatch = Stopwatch.StartNew();
                var result = await httpClient.GetAsync(service.Url);
                var answer = await result.Content.ReadAsStringAsync();
                long sec = stopWatch.ElapsedMilliseconds;
                
                return new Event
                {
                    Available = result.StatusCode == System.Net.HttpStatusCode.OK,
                    EventTime = DateTime.Now,
                    ResponseTime = sec,
                    Service = service,
                };
            }
        }
    }
}
