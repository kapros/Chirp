using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Configuration
{
    public class AzureServiceBusQueueSettings
    {
        public string Address { get; set; }

        public string Topic { get; set; }
    }
}
