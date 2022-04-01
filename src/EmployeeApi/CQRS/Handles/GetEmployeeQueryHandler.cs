using Employee.Api.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api
{
    internal sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeMvc>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public GetEmployeeQueryHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<EmployeeMvc?> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        => mapper.Map(await employeeRepository.EmployeeByLoginAsync(request.Login));
     
    }
}
