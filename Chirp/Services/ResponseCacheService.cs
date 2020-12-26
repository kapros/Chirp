using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _cache;

        public ResponseCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
                return;

            var serializedResponse = JsonConvert.SerializeObject(response);

            await _cache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _cache.GetStringAsync(cacheKey);

            return string.IsNullOrWhiteSpace(cachedResponse) ? null : cachedResponse;
        }
    }
}
