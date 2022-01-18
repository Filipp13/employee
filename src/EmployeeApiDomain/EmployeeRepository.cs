using Employee.Api.Infra;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Api.Domain
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PracticeManagementContext practiceManagementContext;
        private readonly PeopleContext peopleContext;

        public EmployeeRepository(
            PracticeManagementContext practiceManagementContext,
            PeopleContext peopleContext)
        {
            this.practiceManagementContext = practiceManagementContext;
            this.peopleContext = peopleContext;
        }

        public async Task<EmployeeDto> EmployeeByLoginAsync(string login)
        => (await practiceManagementContext.Employees
            .FirstOrDefaultAsync(e => e.AccountName.Equals(login))).Map();

        public Task<List<EmployeeDto>> EmployeesByLoginsAsync(IEnumerable<string> logins)
        => practiceManagementContext.Employees
            .Where(emp => logins.Contains(emp.AccountName))
            .Select(emp => emp.Map())
            .ToListAsync();

        public async Task<Dictionary<string, EmployeeSAPDto>> GetEmployeeSAPDtoAsync()
        => await peopleContext.VPersonDepAndBus
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
                .WhenMatched(empl => new Infra.Employee
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
