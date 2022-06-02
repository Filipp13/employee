using AspNetCore.Cache;
using MediatR;
using System;
using System.Collections.Generic;

namespace Employee.Api
{
    public sealed class GetEmployeesForRoleQuery : IRequest<IEnumerable<EmployeeForRole>>, ICacheableMediatrQuery
    {
        public GetEmployeesForRoleQuery(
            string search,
            string roleCode,
            int armsid,
            string armsListId,
            bool bypassCache = false,
            TimeSpan? slidingExpiration = null)
        {
            Search = search;
            RoleCode = roleCode;
            Armsid = armsid;
            ArmsListId = armsListId;
            BypassCache = bypassCache;
            SlidingExpiration = slidingExpiration;
        }

        public string Search { get; set; }

        public string RoleCode { get; set; }

        public int Armsid { get; set; }

        public string ArmsListId { get; set; }

        public bool BypassCache { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string CacheKey => $"SearchEmployeeForRole-{Search}{RoleCode}{Armsid}{ArmsListId}";
    }
}
