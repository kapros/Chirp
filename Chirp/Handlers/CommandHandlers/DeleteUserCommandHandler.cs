using Chirp.Commands;
using MediatR;
using System;
using Chirp.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chirp.Contracts;

namespace Chirp.Handlers.CommandHandlers
{
    public class DeleteUserCommandHandler : INotificationHandler<DeleteUserCommand>
    {
        public const string EventName = "UserDeleted";

        private const string Version = "1.0";

        private readonly IEventClient _eventPublisher;

        public DeleteUserCommandHandler(IEventClient eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(DeleteUserCommand notification, CancellationToken cancellationToken)
        {
            var @event = new Event<DeleteUserCommand>
            {
                Message = notification,
                EventName = EventName,
                Version = Version
            };
            await _eventPublisher.PublishEvent(@event);
        }
    }
}
