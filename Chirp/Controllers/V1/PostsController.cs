using System;
using System.Threading.Tasks;
using Chirp.Cache;
using Chirp.Commands;
using Chirp.Contracts;
using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Requests.Queries;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using Chirp.Extensions;
using Chirp.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Poster")]
    [Route(ApiRoutes.Posts.PostsRoot)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserId _userId;

        public PostsController(IMediator mediator, UserId userId)
        {
            _mediator = mediator;
            _userId = userId;
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
        [Cached(600)]
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
            var createPostCommand = new CreatePostCommand(_userId)
            {
                CreatePostRequest = postRequest,
                CreatedBy = HttpContext.GetUserId(),
            };

            var createdPost = await _mediator.Send(createPostCommand);

            return Accepted(createdPost);
        }

        /// <summary>
        /// Update a post.
        /// </summary>
        /// <param name="id">Id of the post to update.</param>
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
        [CacheRefresh]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPostQuery = new UserOwnsPostQuery
            {
                PostId = id,
                UserId = HttpContext.GetUserId()
            };

            var userOwnsPostResult = await _mediator.Send(userOwnsPostQuery);
            if (!userOwnsPostResult.Success)
            {
                if (!userOwnsPostResult.PostFound)
                    return NotFound();

                if (!userOwnsPostResult.UserOwnsPost)
                    return BadRequest(new { Error = "You do not own this post" });
            }
            var updatePostCommand = new UpdatePostCommand(_userId)
            {
                PostId = id,
                Update = request
            };

            var accepted = await _mediator.Send(updatePostCommand);

            return Accepted(accepted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("id")]
        [Authorize(Roles = "Admin", Policy = "MustWorkForMe")]
        [CacheRefresh]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletePostCommand = new DeletePostCommand
            {
                PostId = id
            };

            var deleted = await _mediator.Send(deletePostCommand);

            if (deleted)
                return NoContent();

            return BadRequest(new { Error = "Could not delete post" });
        }
    }
}