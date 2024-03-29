﻿
using System.Text.Json.Serialization;

namespace Employee.Api
{
    public sealed class EmployeeForRole
    {
        public EmployeeForRole(EmployeeMvc employee)
        {
            Employee = employee;
        }

        public EmployeeMvc Employee { get; set; }

        [JsonPropertyName("canBeAssignedToRole")]
        public bool CanBeAssignedToRole { get; set; }

        public string Explanation { get; set; } = string.Empty;

    }
}
