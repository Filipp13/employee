using EmployeeApi.Domain;
using System.Collections.Generic;

namespace EmployeeApi
{
    public static class Mapper
    {
        private const string Unknown = "Unknown";

        public static EmployeeUpdateDto Map(this EmployeeAD e, Dictionary<string, EmployeeSAPDto> dictEmployees)
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
    }
}
