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
        private readonly IMapper mapper;

        public GetEmployeesByLoginsQueryHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeMvc?>> Handle(GetEmployeesByLoginsQuery request, CancellationToken cancellationToken)
        => (await employeeRepository.EmployeesByLoginsAsync(request.Logins))
                                    .Select(emp => mapper.Map(emp));
    }
}
