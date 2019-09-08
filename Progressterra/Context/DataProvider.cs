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
        public DataProvider(ServiceEventsHandler serviceEventsHandler)
        {
            this.serviceEventsHandler = serviceEventsHandler;
        }

        public async Task<List<ServiceStatus>> InterrogateServices()
        {
            return await serviceEventsHandler.InterrogateServices();
        }
    }
}
