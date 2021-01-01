using Chirp.Contracts;
using MediatR;


namespace Chirp.Queries
{
    public class PaginatedQuery<T> : IRequest<T>
    {
        public PaginationQuery Pagination { get; set; }

        public IRequest<T> Query { get; set; }
    }
}
