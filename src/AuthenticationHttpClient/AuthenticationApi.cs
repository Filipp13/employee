using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationHttpClient
{
    public class AuthenticationApi : IAuthenticationApi
    {
        private readonly HttpClient client;

        public AuthenticationApi(HttpClient client)
        {
            this.client = client;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var json = JsonSerializer.Serialize(new JsonToken(token));
            return await client.PostAsync("/auth/api/v1/validate", new StringContent(json)) switch
            {
                var response when response.StatusCode == HttpStatusCode.OK => true,
                _ => false
            };
        }
    }
}
