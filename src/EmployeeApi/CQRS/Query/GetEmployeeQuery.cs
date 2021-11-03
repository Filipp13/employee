using EmployeeApi.Domain;
using MediatR;
using System;

namespace Employee
{
    public sealed class GetEmployeeQuery : IRequest<EmployeeDto>, ICacheableMediatrQuery
    {
        public GetEmployeeQuery(
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

        public string CacheKey => $"Employee-{Login}";
    }
}
