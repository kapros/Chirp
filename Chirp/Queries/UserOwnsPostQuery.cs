using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Queries
{
    public class UserOwnsPostQuery : IRequest<(bool Success, bool PostFound, bool UserOwnsPost)>
    {
        public Guid PostId { get; set; }
        public string UserId { get; set; }
    }
}
