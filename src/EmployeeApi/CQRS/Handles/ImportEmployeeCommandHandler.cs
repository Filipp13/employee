using Employee.Api.Domain;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api
{
    public class ImportEmployeeCommandHandler : IRequestHandler<ImportEmployeeCommand, int>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IADManagmentEntity<EmployeeAD, EmployeeMapper> aDManagmentEntity;
        private readonly IMapper mapper;

        public ImportEmployeeCommandHandler(
            IEmployeeRepository employeeRepository, 
            IADManagmentEntity<EmployeeAD, EmployeeMapper> aDManagmentEntity,
            IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.aDManagmentEntity = aDManagmentEntity;
            this.mapper = mapper;
        }

        public async Task<int> Handle(ImportEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeDictinarySap = await employeeRepository.GetEmployeeSAPDtoAsync();

            var employeesMaped = (await aDManagmentEntity.GetEntityOfT())
                .Select(e => mapper.Map(e, employeeDictinarySap));

            return await employeeRepository.UpdateEmployeesAsync(employeesMaped);
        }

    }
}
