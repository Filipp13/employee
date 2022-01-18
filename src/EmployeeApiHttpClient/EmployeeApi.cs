using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Employee.Api.ServiceClient
{
    public sealed class EmployeeApi : IEmployeeApi
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions serializerOptions;

        public EmployeeApi(HttpClient client)
        {
            this.client = client;
            serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<EmployeeSC?> GetEmployeeByLoginAsync(string login)
        {

            var json = await client.GetStringAsync(
               $"/api/employee/user-info/{login}")
               .ConfigureAwait(false);

            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            return await JsonSerializer.DeserializeAsync<EmployeeSC>(stream, serializerOptions)
                .ConfigureAwait(false);
        }

        public Task<EmployeeSC?> GetUserInfoAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAdminAsync(string login)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsRiskManagementAsync(string login)
        {
            throw new System.NotImplementedException();
        }
    }
}
