﻿using EmployeeApi.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi
{
    internal sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeMvc>
    {
        private readonly IEmployeeRepository employeeRepository;

        public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<EmployeeMvc> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        => (await employeeRepository.EmployeeByLoginAsync(request.Login)).Map();
     
    }
}
