using Chirp.Commands;
using Chirp.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chirp.Handlers.CommandHandlers
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IPostService _postService;

        public DeletePostCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _postService.DeletePostAsync(request.PostId);
            return deleted;
        }
    }
}
