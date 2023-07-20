using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TaskProximity.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;

        public EmailNotificationService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, string senderEmail)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _senderEmail = senderEmail;
        }

        public async Task SendEmailNotification(string recipientEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                using (var message = new MailMessage(_senderEmail, recipientEmail))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
