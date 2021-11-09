using System;

namespace EmployeeApi
{
    public interface ICacheableMediatrQuery
    {
        bool BypassCache { get; }

        string CacheKey { get; }

        TimeSpan? SlidingExpiration { get; }
    }
}
