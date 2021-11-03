using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Employee
{
    public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
    {
        private readonly IDistributedCache cache;
        private readonly ILogger logger;
        private readonly CacheSettings settings;
        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            this.cache = cache;
            this.logger = logger;
            this.settings = settings.Value;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;
            if (request.BypassCache) return await next();
            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                var slidingExpiration = request.SlidingExpiration == null ? TimeSpan.FromHours(settings.SlidingExpiration) : request.SlidingExpiration;
                var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response));
                await cache.SetAsync((string)request.CacheKey, serializedData, options, cancellationToken);
                return response;
            }
            var cachedResponse = await cache.GetAsync((string)request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
                logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }
            return response;
        }
    }
}
