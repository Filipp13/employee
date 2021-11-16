using EmployeeApi.Domain;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi
{
    public class ImportEmployeeCommandHandler : IRequestHandler<ImportEmployeeCommand, int>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IADManagmentEntity<EmployeeAD, EmployeeMapper> aDManagmentEntity;

        public ImportEmployeeCommandHandler(
            IEmployeeRepository employeeRepository, 
            IADManagmentEntity<EmployeeAD, EmployeeMapper> aDManagmentEntity)
        {
            this.employeeRepository = employeeRepository;
            this.aDManagmentEntity = aDManagmentEntity;
        }

        public async Task<int> Handle(ImportEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeDictinarySap = await employeeRepository.GetEmployeeSAPDtoAsync();

            var employeesMaped = (await aDManagmentEntity.GetEntityOfT())
                .Select(e => e.Map(employeeDictinarySap));

            return await employeeRepository.UpdateEmployeesAsync(employeesMaped);
        }

    }
}
