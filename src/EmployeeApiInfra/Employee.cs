using System;
using System.Collections.Generic;

#nullable disable

namespace Employee.Api.Infra
{
    public partial class Employee
    {
        public Employee()
        {
            InverseManager = new HashSet<Employee>();
        }

        public int EmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public byte[] ObjectSid { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Grade { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string AccountName { get; set; }
        public string Department { get; set; }
        public string NameFirstLocal { get; set; }
        public string NameLastLocal { get; set; }
        public string DisplayNameLocal { get; set; }
        public string OfficeCity { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
    }
}
