using System.Threading.Tasks;
using DragonflyTracker.Domain;

namespace DragonflyTracker.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> RegisterAsync(string username, string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<AuthenticationResult> LogoutAsync(string refreshToken);
        Task<AuthenticationResult> UpdatePasswordAsync(DragonflyUser userToUpdate, string newPassword);
        Task<AuthenticationResult> ResetPasswordEmailAsync(DragonflyUser user);
        Task<AuthenticationResult> ResetPasswordAsync(DragonflyUser user, string token, string newPassword);
        Task<AuthenticationResult> CheckUserPasswordAsync(DragonflyUser user, string password);
        Task<AuthenticationResult> ValidatePasswordAsync(string password);
        Task<AuthenticationResult> ConfirmEmailAsync(DragonflyUser user, string token);
        Task<AuthenticationResult> SendConfirmationEmailAsync(DragonflyUser user);
    }
}