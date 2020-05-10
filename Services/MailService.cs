using DragonflyTracker.Domain;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
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
            /*
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("ef4c67a28812e2", "7d24b0f5420b5c"),
                EnableSsl = true
            };*/
            foreach (string recipient in recipients) {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Dragonfly", sender));
                message.To.Add(new MailboxAddress(recipient));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = content
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.mailtrap.io", 2525, true).ConfigureAwait(false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("ef4c67a28812e2", "7d24b0f5420b5c").ConfigureAwait(false);

                    await client.SendAsync(message).ConfigureAwait(false);
                    client.Disconnect(true);
                }
            }
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
