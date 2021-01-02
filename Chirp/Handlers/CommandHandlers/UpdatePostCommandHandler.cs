using Chirp.Commands;
using Chirp.Contracts;
using Chirp.Contracts.V1.Responses;
using Chirp.Data;
using Chirp.Domain;
using Chirp.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chirp.Handlers.CommandHandlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Accepted>
    {
        private const string CommandVersion = "1.0";
        private const string EventName = "PostUpdate";

        private readonly IPostService _postService;
        private readonly IUriService _uriService;
        private readonly ISerializationService _serializationService;
        private readonly AcceptedJobsContext _acceptedJobsContext;
        private readonly IEventPublishingService _eventPublishingService;

        public UpdatePostCommandHandler(
            IPostService postService,
            IUriService uriService,
            ISerializationService serializationService,
            AcceptedJobsContext acceptedJobsContext,
            IEventPublishingService eventPublishingService)
        {
            _postService = postService;
            _uriService = uriService;
            _serializationService = serializationService;
            _acceptedJobsContext = acceptedJobsContext;
            _eventPublishingService = eventPublishingService;
        }

        public async Task<Accepted> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetPostByIdAsync(request.PostId);
            var dateStarted = DateTime.UtcNow;
            post.Name = request.Update.Name;
            var tags = request.Update.Tags
                .Select(async x => await _postService.GetTagByNameAsync(x))
                .Select(x => x.Result)
                .Select(x => new PostTag { Id = x.Id, PostId = post.Id })
                .ToList();
            post.Tags = tags;
            var payload = await _serializationService.Serialize(post);
            _acceptedJobsContext.Jobs.Add(new AcceptedJob
            {
                Controller = "Posts",
                DateStarted = dateStarted,
                JobId = request.RequestId,
                UserId = request.UserId,
                Payload = payload
            });
            var @event = new Event<Post>
            {
                Message = post,
                Version = CommandVersion,
                EventId = request.RequestId,
                EventName = EventName
            };
            await _eventPublishingService.SendEvent(@event);
            return new Accepted
            {
                Id = request.RequestId,
                Address = _uriService.Accepted(request.RequestId).ToString()
            };
        }
    }
}
