using Chirp.Contracts.V1.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Queries
{
    public class GetPostByIdQuery : IRequest<PostResponse>
    {
        public Guid PostId { get; set; }
    }
}
