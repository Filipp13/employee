using EmployeeApi.Infra;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Domain
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PracticeManagementContext practiceManagementContext;

        public EmployeeRepository(PracticeManagementContext practiceManagementContext)
        {
            this.practiceManagementContext = practiceManagementContext;
        }

        public async Task<EmployeeDto> EmployeeByLoginAsync(string login)
        => (await practiceManagementContext.Employees
            .FirstOrDefaultAsync(e => e.AccountName.Equals(login))).Map();

        public async Task<Dictionary<string, EmployeeSAPDto>> GetEmployeeSAPDtoAsync()
        => await practiceManagementContext.VPersonDepAndBus
            .Select(p => p.Map())
            .ToDictionaryAsync(p => p.AccountName);

        public Task<List<EmployeeDto>> SearchEmployeeByDisplayNameAsync(string search)
        => practiceManagementContext.Employees
                .Where(e => EF.Functions.Like(e.DisplayName, $"%{search}%") && e.IsActive)
                .OrderBy(e => e.DisplayName)
                .Take(10)
                .Select(e => e.Map())
                .ToListAsync();

        public async Task<int> UpdateEmployeesAsync(IEnumerable<EmployeeUpdateDto> employees)
        => await practiceManagementContext.Employees
                .UpsertRange(employees.Select(e => e.Map()))
                .On(v => new { v.AccountName, v.ObjectSid })
                .WhenMatched(empl => new Employee
                {
                    LastName = empl.LastName,
                    FirstName = empl.FirstName,
                    DisplayName = empl.DisplayName,
                    Email = empl.Email,
                    Title = empl.Title,
                    OfficeCity = empl.OfficeCity,
                    Grade = empl.Grade,
                    IsActive = empl.IsActive,
                    Department = empl.Department,
                    NameFirstLocal = empl.NameFirstLocal,
                    DisplayNameLocal = empl.DisplayNameLocal,
                    NameLastLocal = empl.NameLastLocal
                })
                .RunAsync();
    }
}
