﻿namespace Employee.Api
{
    public interface IRolesManagment
    {
        bool IsADRole(string role);

        bool IsSPRole(string role);

        string SPRole(string role);
    }
}