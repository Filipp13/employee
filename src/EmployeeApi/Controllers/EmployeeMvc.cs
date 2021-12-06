using System.Text.Json.Serialization;

namespace EmployeeApi
{
    public class EmployeeMvc
    {
        public EmployeeMvc(
            int id, 
            string surname,
            string firstName,
            string displayName,
            string title,
            string email, 
            string photoURL,
            string department,
            string accountName,
            bool isActive, 
            string officeCity)
        {
            Id = id;
            Surname = surname;
            FirstName = firstName;
            DisplayName = displayName;
            Title = title;
            Email = email;
            PhotoURL = photoURL;
            Department = department;
            AccountName = accountName;
            IsActive = isActive;
            OfficeCity = officeCity;
        }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("photoUrl")]
        public string PhotoURL { get; set; }

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
