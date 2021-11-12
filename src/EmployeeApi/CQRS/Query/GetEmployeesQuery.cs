﻿using EmployeeApi.Domain;
using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeApi
{
    public sealed class GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>, ICacheableMediatrQuery
    {
        public GetEmployeesQuery(
            string search, 
            bool bypassCache = false, 
            TimeSpan? slidingExpiration = null)
        {
            Search = search;
            BypassCache = bypassCache;
            SlidingExpiration = slidingExpiration;
        }

        public string Search { get; set; }

        public bool BypassCache { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public string CacheKey => $"SearchEmployee-{Search}";
    }
}