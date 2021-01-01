using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IMessageQueueService
    {
        Task<bool> Publish<T>(T message);

        Task<bool> Publish(string raw);
    }
}
