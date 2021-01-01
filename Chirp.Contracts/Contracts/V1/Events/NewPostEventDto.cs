using System;
using System.Collections.Generic;
using System.Text;

namespace Chirp.Contracts.V1.Events
{
    public class NewPostEventDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
