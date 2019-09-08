using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
            //UriBuilder builder = new UriBuilder("http://localhost:6598/api/get");
            //builder.Query = "kk";

            using (var httpClient = new HttpClient())
            {
                var stopWatch = Stopwatch.StartNew();
                var result = await httpClient.GetAsync(service.Url);
                long sec = stopWatch.ElapsedMilliseconds;



                //var json = await result.Content.ReadAsStringAsync();



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
