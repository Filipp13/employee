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

        public async Task<Employee?> GetEmployeeByLoginAsync(string login)
        {

            var json = await client.GetStringAsync(
               $"/api/employee/user-info/{login}")
               .ConfigureAwait(false);

            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            return await JsonSerializer.DeserializeAsync<Employee>(stream, serializerOptions)
                .ConfigureAwait(false);
        }

       

        public Task<Employee?> GetUserInfoAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
