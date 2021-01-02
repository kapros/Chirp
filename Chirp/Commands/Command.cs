using Chirp.Domain;
using System;

namespace Chirp.Commands
{
    public class Command
    {
        public Command(UserId userId)
        {
            UserId = userId.Id;
        }
        public Guid UserId { get; set; }

        public string RequestId { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
    }
}