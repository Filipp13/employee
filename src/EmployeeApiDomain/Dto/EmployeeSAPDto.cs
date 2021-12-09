namespace Employee.Api.Domain
{
    public class EmployeeSAPDto
    {
        public EmployeeSAPDto(
            string department,
            string accountName,
            string nameFirstRu,
            string nameLastRu)
        {
            Department = department;
            AccountName = accountName;
            NameFirstRu = nameFirstRu;
            NameLastRu = nameLastRu;
        }

        public string Department { get; set; }

        public string AccountName { get; set; }

        public string NameFirstRu { get; set; }

        public string NameLastRu { get; set; }

    }

}
