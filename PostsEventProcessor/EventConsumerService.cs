using Chirp.Contracts;
using Chirp.Contracts.V1.Events;
using Chirp.Data;
using Chirp.Domain;
using Mapster;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public class EventConsumerService : IEventConsumerService
    {
        private readonly ISubscriptionClient _subscriptionClient;
        private readonly IPostService _postService;
        private readonly AcceptedJobsContext _acceptedJobsContext;
        private readonly ILogger _logger;

        public EventConsumerService(
            ISubscriptionClient subscriptionClient,
            IPostService postService,
            AcceptedJobsContext acceptedJobsContext,
            ILogger logger)
        {
            _subscriptionClient = subscriptionClient;
            _postService = postService;
            _acceptedJobsContext = acceptedJobsContext;
            _logger = logger;
        }

        public async Task Subscribe()
        {
            _subscriptionClient.RegisterMessageHandler(async (message, token) =>
            {
                var postCreated = JsonConvert.DeserializeObject<Event<NewPostEventDto>>(Encoding.UTF8.GetString(message.Body));
                await CreatePost(postCreated, _postService, _acceptedJobsContext);
                await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            },
            new MessageHandlerOptions(async ex =>
            {
                await _logger.LogExceptionAsync(ex.Exception);
            })
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            });
        }

        public async Task ProcessNewPostAsync(Message message, CancellationToken token)
        {
            var postCreated = JsonConvert.DeserializeObject<Event<NewPostEventDto>>(Encoding.UTF8.GetString(message.Body));
            await CreatePost(postCreated, _postService, _acceptedJobsContext);
        }

        private static CreatePostDto MapPost(NewPostEventDto eventDto)
        {
            return new CreatePostDto
            {
                Post = eventDto.Adapt<Post>(),
                Tags = eventDto.Tags
            };
        }

        private static async Task CreatePost(Event<NewPostEventDto> eventDto, IPostService postService, AcceptedJobsContext acceptedJobsContext)
        {
            var post = await postService.Create(MapPost(eventDto.Message));
            var job = await acceptedJobsContext.Jobs.FirstOrDefaultAsync(x => x.JobId == eventDto.EventId);
            job.DateFinished = DateTime.UtcNow;
            job.CreatedObjectId = post.Id;
            await acceptedJobsContext.SaveChangesAsync();
        }
    }
}
