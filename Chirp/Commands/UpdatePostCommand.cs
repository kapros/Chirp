using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Commands
{
    public class UpdatePostCommand : Command, IRequest<Accepted>
    {
        public UpdatePostCommand(UserId userId) : base(userId)
        {
        }

        public Guid PostId { get; set; }
        public UpdatePostRequest Update { get; set; }
    }
}
