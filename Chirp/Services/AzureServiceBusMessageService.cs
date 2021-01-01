using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class AzureServiceBusMessageService : IMessageQueueService
    {
        private readonly IQueueClient _queueClient;

        public AzureServiceBusMessageService(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task<bool> Publish<T>(T obj)
        {
            try
            {
                var serializedMessage = JsonConvert.SerializeObject(obj);
                var message = new Message(Encoding.UTF8.GetBytes(serializedMessage));
                await _queueClient.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                //logging here
                return false;
            }
        }

        public async Task<bool> Publish(string raw)
        {
            var message = new Message(Encoding.UTF8.GetBytes(raw));

            try
            {
                await _queueClient.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                //logging here
                return false;
            }
        }
    }
}
