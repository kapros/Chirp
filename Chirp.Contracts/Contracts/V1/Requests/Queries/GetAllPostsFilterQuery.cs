using System;
using System.Collections.Generic;
using System.Text;

namespace Chirp.Contracts.V1.Requests.Queries
{
    public class GetAllPostsFilterQuery
    {
        public Guid UserId { get; set; }
    }
}
