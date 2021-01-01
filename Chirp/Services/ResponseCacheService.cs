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
        private readonly ISerializationService _serializationService;

        public ResponseCacheService(IDistributedCache cache, ISerializationService serializationService)
        {
            _cache = cache;
            _serializationService = serializationService;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
                return;

            var serializedResponse = await _serializationService.Serialize(response);

            await _cache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<T> GetCachedResponseAsync<T>(string cacheKey)
        {
            var cachedResponseString = await _cache.GetStringAsync(cacheKey);

            var cachedResponse = await _serializationService.Deserialize<T>(cachedResponseString);

            return cachedResponse;
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponseBytes = await _cache.GetStringAsync(cacheKey);

            var cachedResponse = await _serializationService.GetString(cachedResponseBytes);

            return cachedResponse;
        }
    }
}
