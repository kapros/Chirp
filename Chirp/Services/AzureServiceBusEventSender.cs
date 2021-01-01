using Chirp.Contracts;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class AzureServiceBusEventSender : IEventClient
    {
        private readonly TopicClient _client;

        public AzureServiceBusEventSender(TopicClient client)
        {
            _client = client;
        }

        public async Task<bool> PublishEvent<TEvent>(Event<TEvent> @event)
        {
            var sessionId = Guid.NewGuid().ToString();

            var message = ToMessage(@event, sessionId);
            try
            {
                await _client.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                await _client.CloseAsync();
            }
        }

        public async Task<bool> PublishEvents<TEvent>(IEnumerable<Event<TEvent>> events)
        {
            var sessionId = Guid.NewGuid().ToString();

            var messages = events.Select(x => ToMessage(x, sessionId)).ToList();
            try
            {
                await _client.SendAsync(messages);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                await _client.CloseAsync();
            }
        }

        private Message ToMessage<TEvent>(Event<TEvent> @event, string sessionId)
        {
            var body = SerializeBody(@event);
            var message = new Message(body)
            {
                ContentType = "application/json",
                MessageId = @event.EventId ?? Guid.NewGuid().ToString(),
                SessionId = sessionId
            };
            return message;
        }

        private byte[] SerializeBody<TEvent>(Event<TEvent> @event)
        {
            string serialized = JsonConvert.SerializeObject(@event, Formatting.None);
            return Encoding.UTF8.GetBytes(serialized);
        }

    }
}
