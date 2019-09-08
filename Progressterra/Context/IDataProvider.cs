using Progressterra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Context
{
    public interface IDataProvider
    {
        Task<List<ServiceStatus>> InterrogateServices();
        long GetMaxResponseTime();
    }
}
