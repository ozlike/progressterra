using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Progressterra.Context
{
    public class Header
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Service Service { get; set; }
        public int ServiceId { get; set; }
    }
}
