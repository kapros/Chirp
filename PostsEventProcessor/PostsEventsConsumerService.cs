using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public class PostsEventsConsumerService : BackgroundService
    {
        private readonly IEventConsumerService _eventConsumerService;

        public PostsEventsConsumerService(IEventConsumerService eventConsumerService)
        {
            _eventConsumerService = eventConsumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventConsumerService.Subscribe();
        }
    }
}
