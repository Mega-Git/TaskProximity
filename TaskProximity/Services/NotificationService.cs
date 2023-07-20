using System.Threading.Tasks;
using TaskProximity.Data;
using TaskProximity.Models;

namespace TaskProximity.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TaskProximityDbContext _context;

        public NotificationService(TaskProximityDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotification(int userId, string message)
        {
            var notification = new Notification
            {
                RecipientUserId = userId,
                Message = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}