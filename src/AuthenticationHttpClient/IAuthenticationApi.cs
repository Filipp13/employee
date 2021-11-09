using System.Threading.Tasks;

namespace AuthenticationHttpClient
{
    public interface IAuthenticationApi
    {
        Task<bool> ValidateTokenAsync(string token);
    }
}
