using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi.Domain
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> EmployeeByLoginAsync(string login);

        Task<List<EmployeeDto>> SearchEmployeeByDisplayNameAsync(string search);

        Task<int> UpdateEmployeesAsync(IEnumerable<EmployeeUpdateDto> employees);

        Task<Dictionary<string, EmployeeSAPDto>> GetEmployeeSAPDtoAsync();
    }
}
