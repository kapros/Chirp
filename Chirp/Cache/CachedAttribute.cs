using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Chirp.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        public const int DefaultTimeToLiveInSeconds = 600;

        private readonly int _timeToLiveInSeconds;

        public CachedAttribute() : this(DefaultTimeToLiveInSeconds) { }

        public CachedAttribute(int timeToLive)
        {
            _timeToLiveInSeconds = timeToLive;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = context.HttpContext.Request.GenerateCacheKeyFromRequest();

            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult ok)
            {
                await cacheService.CacheResponseAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }
    }
}
