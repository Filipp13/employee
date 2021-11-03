using System;

namespace Employee
{
    public interface ICacheableMediatrQuery
    {
        bool BypassCache { get; }

        string CacheKey { get; }

        TimeSpan? SlidingExpiration { get; }
    }
}
