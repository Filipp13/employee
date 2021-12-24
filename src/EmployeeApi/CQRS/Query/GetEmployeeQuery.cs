using AspNetCore.Cache;
using MediatR;
using System;

namespace Employee.Api
{
    public sealed class GetEmployeeQuery : IRequest<EmployeeMvc?>, ICacheableMediatrQuery
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
