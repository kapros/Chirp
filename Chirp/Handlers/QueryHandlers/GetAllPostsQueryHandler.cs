using Chirp.Queries;
using Chirp.Contracts.V1.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chirp.Services;
using AutoMapper;
using Chirp.Domain;
using Chirp.Contracts;
using Chirp.Helpers;

namespace Chirp.Handlers.QueryHandlers
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PagedResponse<PostResponse>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public GetAllPostsQueryHandler(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<PagedResponse<PostResponse>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var pagination = _mapper.Map<PaginationFilter>(request.Pagination);

            var filters = _mapper.Map<GetAllPostsFilter>(request.Query);

            var posts = await _postService.GetPostsAsync(filters, pagination);
            var postsResponse = _mapper.Map<List<PostResponse>>(posts);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return new PagedResponse<PostResponse>(postsResponse);

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, postsResponse);

            return paginationResponse;
        }
    }
}
