using Employee.Api.Infra;
using System.Collections.Generic;


namespace Employee.Api.Tests
{
    public static class EmployeesGenerator
    {
        public static List<Infra.Employee> GenerateEmployees(int count)
        {
            var list = new List<Infra.Employee>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Infra.Employee
                {
                    EmployeeId = i,
                    AccountName = $"AccountName{i}",
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    Department = $"Department{i}",
                    DisplayName = $"DisplayName{i}",
                    Email = $"Email{i}",
                    Title = $"Title{i}",
                    IsActive = true
                });
            }
            return list;
        }
    }
}
