using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chirp.Contracts;
using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class TagsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public TagsController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Tags.TagsRoot)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var tags = await _postService.GetAllTagsAsync();
            var tagsResponses = _mapper.Map<List<TagResponse>>(tags);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(new PagedResponse<TagResponse>(tagsResponses));

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, tagsResponses);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Tags.GetByName)]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var tag = await _postService.GetTagByNameAsync(name);

            if (tag == null)
                return NotFound();

            return Ok(new Response<TagResponse>(_mapper.Map<TagResponse>(tag)));
        }

        [HttpPost(ApiRoutes.Tags.TagsRoot)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Error = "Name is empty" });

            var newTag = new Tag
            {
                Name = request.Name,
                CreatedBy = HttpContext.GetUserId(),
                CreatedOn = DateTime.UtcNow
            };

            var created = await _postService.CreateTagAsync(newTag);

            if (!created)
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Failed to created tag" } } });

            var locationUrl = _uriService.UriForGet(newTag.Name);
            return Created(locationUrl, new Response<TagResponse>(_mapper.Map<TagResponse>(newTag)));
        }

        [HttpPut(ApiRoutes.Tags.Put)]
        public async Task<IActionResult> Update([FromRoute] string name, UpdateTagRequest request)
        {
            var tag = await _postService.GetTagByNameAsync(name);

            if (tag == null)
                return NotFound();

            tag.Name = request.NewName;

            var updated = await _postService.UpdateTagAsync(tag);

            if (updated)
                return Ok(new Response<TagResponse>(_mapper.Map<TagResponse>(tag)));

            return NotFound();
        }


        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string tagName)
        {
            var deleted = await _postService.DeleteTagAsync(tagName);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
