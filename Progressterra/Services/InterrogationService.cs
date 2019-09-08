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

            foreach (var service in context.Services)
            {
                events.Add(await MakeQuery(service));
            }

            return events;
        }

        private async Task<Event> MakeQuery(Service service)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                httpClient.DefaultRequestHeaders.Add("AccessKey", "test_05fc5ed1-0199-4259-92a0-2cd58214b29c");
                
                var stopWatch = Stopwatch.StartNew();
                var result = await httpClient.GetAsync(service.Url);
                await result.Content.ReadAsStringAsync();
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
