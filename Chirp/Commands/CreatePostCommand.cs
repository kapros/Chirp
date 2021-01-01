using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Commands
{
    public class CreatePostCommand : IRequest<Accepted>
    {
        public CreatePostCommand()
        {
            RequestId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
        }

        public CreatePostRequest CreatePostRequest { get; set; }

        public string RequestId { get; set; }

        public string CreatedBy { get; set; }
    }
}
