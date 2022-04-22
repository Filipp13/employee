using Employee.Api.Domain;
using EmployeeGrpcService;
using System.Collections.Generic;
using System.Linq;

namespace Employee.Api
{
    public class Mapper : IMapper
    {
        private const string Unknown = "Unknown";

        public EmployeeUpdateDto Map(EmployeeAD e, in Dictionary<string, EmployeeSAPDto> dictEmployees)
        {
            var employee = dictEmployees.GetValueOrDefault(e.SamAccountName);

            var firstName = e?.DisplayName?.Split(", ")?.Length > 1 ? e?.DisplayName?.Split(", ")[1] : string.Empty;
            var lastName = e?.DisplayName?.Split(", ")?[0];

            return new EmployeeUpdateDto(
                e?.ObjectSid!,
                lastName!,
                firstName!,
                e?.DisplayName!,
                e?.Email ?? Unknown,
                e?.Title ?? Unknown,
                e?.SamAccountName!,
                e?.City ?? Unknown,
                e?.Grade!,
                (e?.UserAccountControl / 2) % 2 == 0,
                employee?.Department ?? Unknown,
                employee?.NameFirstRu ?? string.Empty,
                employee?.NameLastRu ?? string.Empty);
        }

        public EmployeeMvc? Map(EmployeeDto employee)
            => employee is null ? default : new EmployeeMvc(employee.Id,
                employee.LastName,
                employee.FirstName,
                employee.DisplayName,
                employee.Title,
                employee.Email,
                employee.PhotoURL,
                employee.Department,
                employee.AccountName,
                employee.IsActive,
                employee.OfficeCity);

        public UserInfoResponse Map(EmployeeMvc employee)
        => new UserInfoResponse()
        {
            Id = employee.Id,
            FirstName = employee.FirstName ?? string.Empty,
            DisplayName = employee.DisplayName ?? string.Empty,
            AccountName = employee.AccountName ?? string.Empty,
            Department = employee.Department ?? string.Empty,
            Email = employee.Email ?? string.Empty,
            IsActive = employee.IsActive,
            OfficeCity = employee.OfficeCity ?? string.Empty,
            PhotoURL = employee.PhotoURL ?? string.Empty,
            Surname = employee.Surname ?? string.Empty,
            Title = employee.Title ?? string.Empty,
        };

        public EmployeesResponse Map(IEnumerable<EmployeeMvc> users)
        {
            var response = new EmployeesResponse();
            response.Users.Add(users.Select(Map));
            return response;
        }
    }
}
