using System;
using System.Collections.Generic;
using Chirp.Contracts;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IEventPublishingService
    {
        Task<bool> SendEvent<T>(Event<T> @event);

        Task<bool> SendEvents<T>(IEnumerable<Event<T>> events);
    }
}
