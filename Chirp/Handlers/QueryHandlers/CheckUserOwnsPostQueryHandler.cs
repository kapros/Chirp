using Chirp.Queries;
using Chirp.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chirp.Handlers.QueryHandlers
{
    public class CheckUserOwnsPostQueryHandler : IRequestHandler<UserOwnsPostQuery, (bool Success, bool PostFound, bool UserOwnsPost)>
    {
        private readonly IPostService _postService;

        public CheckUserOwnsPostQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<(bool Success, bool PostFound, bool UserOwnsPost)> Handle(UserOwnsPostQuery request, CancellationToken cancellationToken)
        {
            var userOwnsPost = await _postService.UserOwnsPost(request.PostId, request.UserId);
            return userOwnsPost;
        }
    }
}
