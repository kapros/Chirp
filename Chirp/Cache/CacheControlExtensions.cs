using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Cache
{
    public static class CacheControlExtensions
    {
        public static string GenerateCacheKeyFromRequest(this HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append(request.Path);

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
