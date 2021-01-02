using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
