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

        public Task<List<EmployeeDto>> SearchEmployeeByDisplayName(string search)
        => practiceManagementContext.Employees
                .Where(e => EF.Functions.Like(e.DisplayName, $"%{search}%") && e.IsActive)
                .OrderBy(e => e.DisplayName)
                .Take(10)
                .Select(e => e.Map())
                .ToListAsync();
    }
}
