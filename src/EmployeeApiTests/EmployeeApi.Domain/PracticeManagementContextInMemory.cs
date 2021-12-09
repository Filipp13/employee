using Employee.Api.Infra;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Employee.Api.Tests
{
    public class PracticeManagementContextInMemory
    {
        public PracticeManagementContext GetPracticeManagementContextInMemory(IEnumerable<Infra.Employee> employees)
        {
            var options = new DbContextOptionsBuilder<PracticeManagementContext>()
               .UseInMemoryDatabase(databaseName: "PracticeManagementDatabase")
               .Options;

            var context = new PracticeManagementContext(options);

            context.Employees.AddRange(employees);
            
            context.SaveChanges();

            return context;
        }
    }
}
