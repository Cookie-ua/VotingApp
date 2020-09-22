using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voting.Core.Configuration;
using Voting.Core.Entities;
using Voting.Core.Interfaces.Services;

namespace Voting.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendMailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await smtpClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                await smtpClient.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await smtpClient.SendAsync(emailMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format($"<h3>{message.Content}<h/3>") };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
    }
}
