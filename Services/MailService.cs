using DragonflyTracker.Domain;
using DragonflyTracker.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonflyTracker.Services
{
    public class MailService : IMailService
    {
        private readonly string _fromName;
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useSsl;

        public MailService(EmailSettings settings)
        {
            if (settings != null)
            {
                _fromName = settings.FromName;
                _username = settings.UserName;
                _password = settings.Password;
                _host = settings.Host;
                _port = settings.Port;
                _useSsl = settings.UseSsl;
            }
        }

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
            foreach (string recipient in recipients)
            {

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_fromName, sender));
                message.To.Add(new MailboxAddress(recipient));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = content
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable).ConfigureAwait(false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_username, _password).ConfigureAwait(false);

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
