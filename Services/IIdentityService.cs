using System.Threading.Tasks;
using DragonflyTracker.Domain;

namespace DragonflyTracker.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        
        Task<AuthenticationResult> LoginAsync(string email, string password);
        
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<AuthenticationResult> LogoutAsync(string refreshToken);
    }
}