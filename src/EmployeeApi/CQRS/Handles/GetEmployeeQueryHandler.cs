using EmployeeApi.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Employee
{
    internal sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository employeeRepository;

        public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        => await employeeRepository.EmployeeByLoginAsync(request.Login);
     
    }
}
