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
        Task<bool> UpdatePasswordAsync(DragonflyUser userToUpdate, string newPassword);
        Task<bool> ResetPasswordEmailAsync(DragonflyUser user);
        Task<bool> ResetPasswordAsync(DragonflyUser user, string token, string newPassword);
        Task<bool> CheckUserPasswordAsync(DragonflyUser user, string password);
        Task<bool> ValidatePasswordAsync(string password);
        Task<bool> ConfirmEmailAsync(DragonflyUser user, string token);
        Task<bool> SendConfirmationEmailAsync(DragonflyUser user);
    }
}