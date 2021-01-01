using Chirp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IUriService
    {
        /// <summary>
        /// Appends the resource ID to the current URI.
        /// </summary>
        Uri UriForGet(object resourceId);

        /// <summary>
        /// Adds pagination query parameters to the current URI.
        /// </summary>
        Uri UriForGetAll(PaginationQuery paginationQuery = null);

        /// <summary>
        /// Returns the URI the consumer should use to query for the newly created resource.
        /// </summary>
        Uri Accepted(object id);
    }
}
