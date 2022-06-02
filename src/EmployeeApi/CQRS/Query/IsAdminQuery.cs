using AspNetCore.Cache;
using MediatR;
using System;

namespace Employee.Api
{
    public sealed class IsAdminQuery : IRequest<bool>, ICacheableMediatrQuery
    {
        public IsAdminQuery(
            string login, 
            bool bypassCache = false, 
            TimeSpan? slidingExpiration = null)
        {
            Login = login;
            BypassCache = bypassCache;
            SlidingExpiration = slidingExpiration;
        }

        public string Login { get; set; }

        public bool BypassCache { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string CacheKey => $"IsAdmin-{Login}";
    }
}
