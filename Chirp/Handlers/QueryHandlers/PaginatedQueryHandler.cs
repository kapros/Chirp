using AutoMapper;
using Chirp.Domain;
using Chirp.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chirp.Handlers.QueryHandlers
{
    public class PaginatedQueryHandler<T> : IRequestHandler<PaginatedQuery<T>, T>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaginatedQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<T> Handle(PaginatedQuery<T> request, CancellationToken cancellationToken)
        {
            var pagination = _mapper.Map<PaginationFilter>(request.Pagination);
            (request.Query as Paged).Pagination = pagination;

            return await _mediator.Send(request.Query);
        }
    }
}
