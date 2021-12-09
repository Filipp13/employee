using System.Threading.Tasks;

namespace Employee.Api.ServiceClient
{
    public interface IEmployeeApi
    {
        Task<EmployeeSC?> GetUserInfoAsync();

        Task<EmployeeSC?> GetEmployeeByLoginAsync(string login);

        Task<bool> IsAdminAsync(string login);

        Task<bool> IsRiskManagementAsync(string login);

    }
}
