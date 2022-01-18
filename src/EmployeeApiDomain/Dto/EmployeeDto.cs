
namespace Employee.Api.Domain
{
    public class EmployeeDto
    {
        private const string V = "https://rumos2420.atrema.deloitte.com/owa/service.svc/s/GetPersonaPhoto?email=";
        private const string V1 = "%40deloitte.ru&size=HR240x240&sc=1571655975940";

        public EmployeeDto(
            int id, 
            string lastName, 
            string firstName, 
            string displayName, 
            string title, 
            string email, 
            string department,
            string accountName, 
            bool isActive, 
            string officeCity)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            DisplayName = displayName;
            Title = title;
            Email = email;
            Department = department;
            AccountName = accountName;
            IsActive = isActive;
            OfficeCity = officeCity;
        }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string DisplayName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string PhotoURL
        {
            get
            {
                if (IsActive)
                    return V + AccountName + V1;
                else
                    return string.Empty;
            }
        }

        public string Department { get; set; }

        public string AccountName { get; set; }

        public bool IsActive { get; set; }

        public string OfficeCity { get; set; }
    }

}
