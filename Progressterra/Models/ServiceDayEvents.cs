using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Models
{
    public class ServiceDayEvents
    {
        public List<Event> Events { get; set; }
        public ServiceStatus Status { get; set; }
    }
}
