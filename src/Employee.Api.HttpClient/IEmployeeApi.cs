using System.Threading.Tasks;

namespace Employee.Api.ServiceClient
{
    public interface IEmployeeApi
    {
        Task<Employee?> GetUserInfoAsync();

        Task<Employee?> GetEmployeeByLoginAsync(string login);
    }
}
