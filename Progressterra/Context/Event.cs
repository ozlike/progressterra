using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Context
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime EventTime { get; set; }
        public long? ResponseTime { get; set; }
        public bool Available { get; set; }

        public Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
