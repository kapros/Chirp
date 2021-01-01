using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chirp.Cache;
using Chirp.Contracts;
using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Requests.Queries;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using Chirp.Extensions;
using Chirp.Helpers;
using Chirp.Queries;
using Chirp.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Poster", Policy = "MustWorkForMe")]
    [Route(ApiRoutes.Posts.PostsRoot)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IMediator _mediator;

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService, IMediator mediator)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
            _mediator = mediator;
        }

        [HttpGet]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostsFilterQuery query, [FromQuery] PaginationQuery paginationQuery)
        {
            var paginatedQuery = new PaginatedQuery<PagedResponse<PostResponse>>
            {
                Pagination = paginationQuery,
                Query = new GetAllPostsQuery
                {
                    Query = query
                }
            };

            var postsResponse = await _mediator.Send(paginatedQuery);

            return Ok(postsResponse);
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var postQuery = new GetPostByIdQuery
            {
                PostId = id
            };

            var post = await _mediator.Send(postQuery);

            return post != null ? Ok(new Response<PostResponse>(post)) : (IActionResult)NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {

            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            await _postService.CreatePostAsync(post);

            var locationUrl = _uriService.UriForGet(post.Id);

            return Accepted(new Accepted
            {
                Id = timestamp,
                Address = locationUrl.ToString()
            });
            return Created(locationUrl, new Response<PostResponse>(_mapper.Map<PostResponse>(post)));
        }

        /// <summary>
        /// Update a post.
        /// </summary>
        /// <param name="id">Id of the psot to update.</param>
        /// <param name="request">The content with which to update.</param>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         POST /api/v1/posts/037E3FB5-25CA-4C1F-A305-A5878451F454
        ///         {
        ///             "Name": "My edited post"
        ///         }
        /// </remarks>
        /// <response code="200">Post was updated.</response>
        /// <response code="400">You do not own the post.</response>
        /// <response code="404">The post does not exist.</response>
        [HttpPut("id")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPost(id, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { Error = "You do not own this post" });

            var post = await _postService.GetPostByIdAsync(id);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(new Response<PostResponse>(_mapper.Map<PostResponse>(post)));

            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var userOwnsPost = await _postService.UserOwnsPost(id, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { Error = "You do not own this post" });

            var deleted = await _postService.DeletePostAsync(id);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}