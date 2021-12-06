using EmployeeApi.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi
{
    internal sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeMvc>>
    {
        private readonly IEmployeeRepository employeeRepository;

        public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeMvc>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        => (await employeeRepository.SearchEmployeeByDisplayNameAsync(request.Search)).Select(e => e.Map());
     
    }
}
