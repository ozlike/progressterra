using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Services
{
    public class ConfigClass
    {
        public long MaxTimeAnswer { get; private set; }
        public int PollingRate { get; private set; }
        public ConfigClass(long maxTimeAnswer, int pollingRate)
        {
            MaxTimeAnswer = maxTimeAnswer;
            PollingRate = pollingRate;
        }
    }
}
