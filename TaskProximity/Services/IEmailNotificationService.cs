using System.Threading.Tasks;

namespace TaskProximity.Services
{
    public interface IEmailNotificationService
    {
        Task SendEmailNotification(string recipientEmail, string subject, string body);
    }
}