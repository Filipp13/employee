namespace Employee.Api.Domain
{
    public class EmployeeUpdateDto
    {
        public EmployeeUpdateDto(
            byte[] objectSid,
            string lastName, 
            string firstName, 
            string displayName, 
            string email, 
            string title,
            string accountName,
            string officeCity,
            string grade,
            bool isActive,
            string department,
            string nameFirstLocal,
            string nameLastLocal)
        {
            ObjectSid = objectSid;
            LastName = lastName;
            FirstName = firstName;
            DisplayName = displayName;
            Email = email;
            Title = title;
            AccountName = accountName;
            OfficeCity = officeCity;
            Grade = grade;
            IsActive = isActive;
            Department = department;
            NameFirstLocal = nameFirstLocal;
            NameLastLocal = nameLastLocal;
        }

        public byte[] ObjectSid { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string DisplayName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public string NameFirstLocal { get; set; }

        public string NameLastLocal { get; set; }

        public string DisplayNameLocal 
        {
            get => $"{NameLastLocal}, {NameFirstLocal}";

        }

        public string AccountName { get; set; }

        public bool IsActive { get; set; }

        public string OfficeCity { get; set; }

        public string Grade { get; set; }

    }

}
