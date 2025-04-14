using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Core.DTO;
using NotificationService.Infrastructure.EmailSender;

namespace NotificationService.Core.Services.SeparateService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendBulkEmailAsync(IEnumerable<PersonalizedEmail> emails);
    }
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendBulkEmailAsync(IEnumerable<PersonalizedEmail> emails)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, _mailSettings.UseSSL);
            await smtp.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);

            foreach (var item in emails)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(item.RecipientEmail, item.RecipientEmail));
                message.Subject = item.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = item.HtmlBody
                };

                message.Body = builder.ToMessageBody();

                try
                {
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {item.RecipientEmail}: {ex.Message}");
                }
            }

            await smtp.DisconnectAsync(true);
        }


        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, _mailSettings.UseSSL);
            await smtp.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
