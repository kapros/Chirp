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
using Chirp.Services;
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

        public PostsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPostsQuery query, [FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var filters = _mapper.Map<GetAllPostsFilter>(query);

            var posts = await _postService.GetPostsAsync(filters, pagination);

            var postsResponse = _mapper.Map<List<PostResponse>>(posts);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(new PagedResponse<PostResponse>(postsResponse));

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, postsResponse);

            return Ok(paginationResponse);
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var post = await _postService.GetPostbyIdAsync(id);

            if (post == null)
                return NotFound();

            var postResponse = _mapper.Map<PostResponse>(post);
            return Ok(new Response<PostResponse>(postResponse));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            await _postService.CreatePostAsync(post);

            var locationUrl = _uriService.GetPostUri(post.Id);

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

            var post = await _postService.GetPostbyIdAsync(id);
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