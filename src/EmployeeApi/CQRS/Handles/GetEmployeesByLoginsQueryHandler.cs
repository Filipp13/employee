using Employee.Api.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api
{
    internal sealed class GetEmployeesByLoginsQueryHandler : IRequestHandler<GetEmployeesByLoginsQuery, IEnumerable<EmployeeMvc>>
    {
        private readonly IEmployeeRepository employeeRepository;

        public GetEmployeesByLoginsQueryHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeMvc?>> Handle(GetEmployeesByLoginsQuery request, CancellationToken cancellationToken)
        => (await employeeRepository.EmployeesByLoginsAsync(request.Logins))
                                    .Select(emp => emp.Map());
    }
}
