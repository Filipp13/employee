using Employee.Api.Domain;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api
{
    internal sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeMvc>>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public GetEmployeesQueryHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeMvc>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        => (await employeeRepository.SearchEmployeeByDisplayNameAsync(request.Search)).Select(mapper.Map);
     
    }
}
