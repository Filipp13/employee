using System.Threading.Tasks;

namespace EmployeeApi
{
    public interface IADManagment
    {
        public Task<bool> IsAdminAsync(string login);

        public Task<bool> IsUsersInsideGroupAsync(string groupName, string login);

        public Task<bool> IsUsersInsideServiceGroupAsync(string groupName, string login);
    }
}
