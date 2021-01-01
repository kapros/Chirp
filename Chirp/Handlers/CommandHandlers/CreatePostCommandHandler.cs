using AutoMapper;
using Chirp.Commands;
using Chirp.Contracts;
using Chirp.Contracts.V1.Events;
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
using ZeroFormatter;

namespace Chirp.Handlers.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Accepted>
    {
        private const string ControllerName = "Posts";
        private const string EventVersion = "1.0";
        private const string EventName = "CreatePost";

        private readonly IMapper _mapper;
        private readonly AcceptedJobsContext _acceptedJobsContext;
        private readonly IEventPublishingService _eventPublishingService;
        private readonly IUriService _uriService;

        public CreatePostCommandHandler(IMapper mapper, AcceptedJobsContext acceptedJobsContext, IEventPublishingService eventPublishingService, IUriService uriService)
        {
            _mapper = mapper;
            _acceptedJobsContext = acceptedJobsContext;
            _eventPublishingService = eventPublishingService;
            _uriService = uriService;
        }

        public async Task<Accepted> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var serializedEvent = ZeroFormatterSerializer.Serialize(request.CreatePostRequest);

            _acceptedJobsContext.Add(new AcceptedJob
            {
                Payload = serializedEvent.ToString(),
                JobId = request.RequestId,
                Controller = ControllerName,
                DateStarted = DateTime.UtcNow
            });

            var post = new NewPostEventDto
            {
                Name = request.CreatePostRequest.Name,
                UserId = request.CreatedBy,
                Tags = request.CreatePostRequest.Tags
            };

            var eventToPublish = new Event<NewPostEventDto>();
            eventToPublish.EventId = request.RequestId;
            eventToPublish.Version = EventVersion;
            eventToPublish.EventName = EventName;
            eventToPublish.Message = post;
            await _eventPublishingService.SendEvent(eventToPublish);

            return new Accepted
            {
                Id = request.RequestId,
                Address = _uriService.Accepted(request.RequestId).ToString()
            };
        }
    }
}
