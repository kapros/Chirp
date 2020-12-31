using AutoMapper;
using Chirp.Contracts.V1.Responses;
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
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostResponse>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);

            return post != null ? _mapper.Map<PostResponse>(post) : null;
        }
    }
}
