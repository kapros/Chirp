using Chirp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class EventPublishingService : IEventPublishingService
    {
        private readonly IEventClient _eventClient;

        public EventPublishingService(IEventClient eventClient)
        {
            _eventClient = eventClient;
        }
        public Task<bool> SendEvent<T>(Event<T> @event)
        {
            return _eventClient.PublishEvent<T>(@event);
        }

        public Task<bool> SendEvents<T>(IEnumerable<Event<T>> events)
        {
            return _eventClient.PublishEvents<T>(events);
        }
    }
}
