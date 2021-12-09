using System;

namespace Employee.Api
{
    public interface ICacheableMediatrQuery
    {
        bool BypassCache { get; }

        string CacheKey { get; }

        TimeSpan? SlidingExpiration { get; }
    }
}
