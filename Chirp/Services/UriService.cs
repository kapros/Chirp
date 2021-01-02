using Chirp.Contracts;
using Chirp.Contracts.V1;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class UriService : IUriService
    {
        private readonly Uri _baseUri;

        private string GetHost() =>
                 string.Concat(_baseUri.Scheme, "://", _baseUri.Authority);

        public UriService(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri Accepted(object id)
        {
            return new Uri(GetHost() + "/api/jobs/" + id.ToString());
        }

        public Uri UriForGet(object resourceId)
        {
            return new Uri(_baseUri + "/" + resourceId.ToString());
        }

        public Uri UriForGetAll(PaginationQuery paginationQuery = null)
        {
            if (paginationQuery == null)
                return _baseUri;

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri.AbsoluteUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
