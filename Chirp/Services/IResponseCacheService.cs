using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task<T> GetCachedResponseAsync<T>(string cacheKey);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
