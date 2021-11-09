using EmployeeApi.Infra;
using System.Collections.Generic;


namespace EmployeeApi.Tests
{
    public static class EmployeesGenerator
    {
        public static List<Employee> GenerateEmployees(int count)
        {
            var list = new List<Employee>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Employee
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
