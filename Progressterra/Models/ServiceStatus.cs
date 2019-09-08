using Progressterra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Models
{
    public class ServiceStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public long? ResponseTime { get; set; }
        public bool Available { get; set; }

        public int FailsInLastHour { get; set; } 
        public long? MaxDeviationInLastHour { get; set; }

        public int FailsInLastDay { get; set; }
        public long? MaxDeviationInLastDay { get; set; }
    }
}
