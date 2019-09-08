using Progressterra.Models;
using Progressterra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Context
{
    public class DataProvider : IDataProvider
    {
        ServiceEventsHandler serviceEventsHandler;
        ProgressterraContext context;
        public DataProvider(ServiceEventsHandler serviceEventsHandler, ProgressterraContext context)
        {
            this.serviceEventsHandler = serviceEventsHandler;
            this.context = context;
        }

        public List<Event> GetEventsForService(int serviceId)
        {
            return context.Events.Where(x => x.ServiceId == serviceId && x.EventTime > DateTime.Now.AddHours(-1))?.OrderByDescending(x => x.EventTime)?.ToList();
        }

        public long GetMaxResponseTime()
        {
            return serviceEventsHandler.MaxResponseTime;
        }

        public async Task<List<ServiceStatus>> InterrogateServices()
        {
            return await serviceEventsHandler.InterrogateServices(false);
        }
    }
}
