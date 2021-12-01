using DeepEqual.Syntax;
using EmployeeApi.Domain;
using EmployeeApi.Infra;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeApi.Tests
{
    public class EmployeeRepositoryTests
    {
        static List<Employee> employes;
        static EmployeeRepository employeeRepository;

        static EmployeeRepositoryTests()
        {
            employes = EmployeesGenerator.GenerateEmployees(8);
            employeeRepository = new EmployeeRepository(
                new PracticeManagementContextInMemory().GetPracticeManagementContextInMemory(employes),
                Mock.Of<PeopleContext>());
        }

        [Fact]
        public async Task EmployeeByLoginAsyncShouldEqualSource()
        {
            

            var empl = employes.First();
            var employee = await employeeRepository.EmployeeByLoginAsync(empl.AccountName);

            employee.ShouldDeepEqual(empl.Map());
        }

        [Fact]
        public async Task EmployeeByLoginAsyncShouldEqualSource1()
        {
            var employee = await employeeRepository.SearchEmployeeByDisplayNameAsync("DisplayName");

            Assert.True(employee.Count == 8);
        }
    }
}
