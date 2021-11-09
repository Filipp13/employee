using System.Text.Json.Serialization;

namespace EmployeeApi.Domain
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

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("surname")]
        public string LastName { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("photoUrl")]
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

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("officeCity")]
        public string OfficeCity { get; set; }
    }

}
