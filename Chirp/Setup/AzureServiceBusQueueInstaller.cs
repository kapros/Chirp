using Chirp.Configuration;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Setup
{
    public class AzureServiceBusQueueInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var queueSettings = new AzureServiceBusQueueSettings();
            configuration.GetSection("Queues:Queue").Bind(queueSettings);

            var queueClient = new QueueClient(queueSettings.Address, queueSettings.Topic, retryPolicy: RetryPolicy.Default);

            services.AddSingleton(queueClient);
        }
    }
}
