using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Domain
{
    public class UserId
    {
        public Guid Id { get; }

        public UserId(Guid userId)
        {
            Id = userId;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
