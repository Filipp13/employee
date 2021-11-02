﻿using EmployeeApi.Domain;
using System.Text.Json.Serialization;

namespace Employee.Controllers
{
    public sealed class EmployeeForRole
    {
        public EmployeeForRole(EmployeeDto employee)
        {
            Employee = employee;
        }

        public EmployeeDto Employee { get; set; }

        [JsonPropertyName("canBeAssignedToRole")]
        public bool CanBeAssignedToRole { get; set; }

        public string Explanation { get; set; } = string.Empty;

    }
}
