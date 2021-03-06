﻿using Chirp.Contracts;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using Chirp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, IEnumerable<T> response)
        {
            var nextPage = pagination.PageNumber >= 1 ? uriService.UriForGetAll(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize)).ToString() : null;

            var previousPage = pagination.PageNumber - 1 >= 1 ? uriService.UriForGetAll(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize)).ToString() : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };
        }
    }
}
