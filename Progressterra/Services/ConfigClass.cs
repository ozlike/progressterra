using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class ConfigClass
    {
        public long MaxResponseTime { get; private set; }
        public int PollingRate { get; private set; }
        public ConfigClass(long maxResponseTime, int pollingRate)
        {
            maxResponseTime = MaxResponseTime;
            PollingRate = pollingRate;
        }
    }
}
