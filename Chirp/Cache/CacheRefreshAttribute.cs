using Chirp.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Cache
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CacheRefreshAttribute : Attribute, IAsyncResultFilter
    {
        public CacheRefreshAttribute()
        {
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = context.HttpContext.Request.GenerateCacheKeyFromRequest();

            await cacheService.InvalidateCache(cacheKey);

            await next();
        }
    }
}
