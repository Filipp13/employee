using Employee.Api.Infra;

namespace Employee.Api.Domain
{
    public static class Mapper
    {
        public static EmployeeDto Map(this Infra.Employee empl)
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

        public static Infra.Employee Map(this EmployeeUpdateDto empl)
        => empl is not null ? new Infra.Employee
        {
            ObjectSid = empl.ObjectSid,
            LastName = empl.LastName,
            FirstName = empl.FirstName,
            DisplayName = empl.DisplayName,
            Email = empl.Email,
            Title = empl.Title,
            AccountName = empl.AccountName,
            OfficeCity = empl.OfficeCity,
            Grade = empl.Grade,
            IsActive = empl.IsActive,
            Department = empl.Department,
            NameFirstLocal = empl.NameFirstLocal,
            DisplayNameLocal = empl.DisplayNameLocal,
            NameLastLocal = empl.NameLastLocal
        } : default!;

        public static EmployeeSAPDto Map(this VPersonDepAndBu p)
        => new EmployeeSAPDto(
            p.Department,
            p.Ad,
            p.NameFirstRu,
            p.NameLastRu);
    }
}
