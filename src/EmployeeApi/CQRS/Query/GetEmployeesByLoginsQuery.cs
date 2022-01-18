using AspNetCore.Cache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Employee.Api
{
    public sealed class GetEmployeesByLoginsQuery : IRequest<IEnumerable<EmployeeMvc>>, ICacheableMediatrQuery
    {
        public GetEmployeesByLoginsQuery(
            IEnumerable<string> logins, 
            bool bypassCache = false, 
            TimeSpan? slidingExpiration = null)
        {
            Logins = logins;
            BypassCache = bypassCache;
            SlidingExpiration = slidingExpiration;
        }

        public IEnumerable<string> Logins { get; set; }

        public bool BypassCache { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string CacheKey => $"GetEmployeesByLogins-{Logins.Aggregate((login1, login2) => $"{login1}-{login2}")}";
    }
}
