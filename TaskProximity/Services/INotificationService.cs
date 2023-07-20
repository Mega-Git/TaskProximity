using System.Threading.Tasks;

namespace TaskProximity.Services
{
    public interface INotificationService
    {
        Task CreateNotification(int userId, string message);
    }
}