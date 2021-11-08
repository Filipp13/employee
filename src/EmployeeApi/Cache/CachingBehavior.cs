using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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

        public CachingBehavior(
            IDistributedCache cache, 
            ILogger<TResponse> logger, 
            IOptions<CacheSettings> settings)
        {
            this.cache = cache;
            this.logger = logger;
            this.settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            if (request.BypassCache) return await next();

            var cachedResponse = await cache.GetAsync(request.CacheKey, cancellationToken);

            if (cachedResponse is not null)
            {
                response = await JsonSerializer.DeserializeAsync<TResponse>(new MemoryStream(cachedResponse))
                    ?? throw new Exception("CachingBehavior DeserializeAsync is null");

                logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {

                response = await next();
                var slidingExpiration = request.SlidingExpiration == null ? TimeSpan.FromHours(settings.SlidingExpiration) : request.SlidingExpiration;
                var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, response);
                await cache.SetAsync(request.CacheKey, stream.ToArray(), options, cancellationToken);

                logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }

            return response;
        }
    }
}
