using EmployeeApi.Infra;

namespace EmployeeApi.Domain
{
    public static class Mapper
    {

        public static EmployeeDto Map(this Employee empl)
        => empl is not null ? new EmployeeDto(
            empl.EmployeeId,
            empl.LastName,
            empl.FirstName,
            empl.DisplayName,
            empl.Title,
            empl.Email,
            empl.Department,
            empl.AccountName,
            empl.IsActive,
            empl.OfficeCity) : default!;
    }
}
