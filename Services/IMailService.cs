using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public interface IMailService
    {
        Task<bool> SendAccountVerificationEmailAsync(DragonflyUser user);
        Task<bool> SendPasswordResetEmailAsync(DragonflyUser user);
        Task<bool> SendPasswordChangedEmailAsync(DragonflyUser user);
        Task<bool> SendEmailChangedEmailAsync(DragonflyUser user, string oldEmail, string newEmail);
        Task<bool> SendEmailAsync(List<string> recipients, string sender, string subject, string textBody, string htmlBody);
    }
}
