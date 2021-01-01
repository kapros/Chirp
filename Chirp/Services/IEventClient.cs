using Chirp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IEventClient
    {
        Task<bool> PublishEvent<TEvent>(Event<TEvent> @event);

        Task<bool> PublishEvents<TEvent>(IEnumerable<Event<TEvent>> events);
    }
}
