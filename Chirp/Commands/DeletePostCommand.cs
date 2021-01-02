using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Commands
{
    public class DeletePostCommand : IRequest<bool>
    {
        public Guid PostId { get; set; }
    }
}
