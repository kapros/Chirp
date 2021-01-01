using Chirp.Contracts;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class AzureEventGridSender : IEventClient
    {
        private readonly EventGridClient _eventGridClient;
        private readonly string _eventGridHost;
        private readonly string _topic;
        private readonly string _subject;

        public AzureEventGridSender(EventGridClient eventGridClient, string eventGridHost, string topic, string subject)
        {
            _eventGridClient = eventGridClient;
            _eventGridHost = eventGridHost;
            _topic = topic;
            _subject = subject;
        }

        public async Task<bool> PublishEvent<TEvent>(Event<TEvent> message)
        {
            try
            {
                var @event = ToEventGridEvent(message);
                var events = new List<EventGridEvent>()
                {
                    @event
                };
                await _eventGridClient.PublishEventsAsync(_eventGridHost, events);
                return true;
            }
            catch (Exception ex)
            {
                // logging here
                return false;
            }
        }

        public async Task<bool> PublishEvents<TEvent>(IEnumerable<Event<TEvent>> events)
        {
            try
            {
                var eventGrindEvents = events.Select(x => ToEventGridEvent(x)).ToList();
                await _eventGridClient.PublishEventsAsync(_eventGridHost, eventGrindEvents);
                return true;
            }
            catch (Exception ex)
            {
                // logging here
                return false;
            }
        }

        private EventGridEvent ToEventGridEvent<TEvent>(Event<TEvent> message)
        {
            return new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                Data = message.Message,
                EventType = message.EventName,
                Topic = _topic,
                EventTime = DateTime.UtcNow,
                DataVersion = message.Version
            };
        }
    }
}
