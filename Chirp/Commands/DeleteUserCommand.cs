using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Commands
{
    public class DeleteUserCommand : INotification
    {
        public string UserId { get; set; }

        public static DeleteUserCommand FromGuid(Guid userId) => new DeleteUserCommand { UserId = userId.ToString() };
    }
}
