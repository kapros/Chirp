using Chirp.Contracts;
using Chirp.Contracts.V1.Requests.Queries;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Queries
{
    public class GetAllPostsQuery : Paged, IRequest<PagedResponse<PostResponse>>
    {
        public GetAllPostsFilterQuery Query { get; set; }
    }
}
