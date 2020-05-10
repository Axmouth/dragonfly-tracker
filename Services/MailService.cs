using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class MailService : IMailService
    {
        public async Task<bool> SendAccountVerificationEmailAsync(DragonflyUser user)
        {
            var result = await SendEmailAsync(new List<string> { "from@example.com" }, "to@example.com", "Hello world", "testbody").ConfigureAwait(false);
            return result;
        }

        public async Task<bool> SendEmailAsync(List<string> recipients, string sender, string subject, string content)
        {
            if (recipients == null)
            {
                return false;
            }
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("ef4c67a28812e2", "7d24b0f5420b5c"),
                EnableSsl = true
            };
            foreach (string recipient in recipients) {
                await client.SendMailAsync(sender, recipient, subject, content).ConfigureAwait(false);
            }
            client.Dispose();
            return true;
        }

        public async Task<bool> SendEmailChangedEmailAsync(DragonflyUser user, string oldEmail, string newEmail)
        {
            var result = await SendEmailAsync(new List<string> { "from@example.com" }, "to@example.com", "Hello world", "testbody").ConfigureAwait(false);
            return result;
        }

        public async Task<bool> SendPasswordChangedEmailAsync(DragonflyUser user)
        {
            var result = await SendEmailAsync(new List<string> { "from@example.com" }, "to@example.com", "Hello world", "testbody").ConfigureAwait(false);
            return result;
        }

        public async Task<bool> SendPasswordResetEmailAsync(DragonflyUser user)
        {
            var result = await SendEmailAsync(new List<string> { "from@example.com" }, "to@example.com", "Hello world", "testbody").ConfigureAwait(false);
            return result;
        }
    }
}
