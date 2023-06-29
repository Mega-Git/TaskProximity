using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProximity.Data;
using TaskProximity.Models;

namespace TaskProximity.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly TaskProximityDbContext _context;

        public NotificationController(TaskProximityDbContext context)
        {
            _context = context;
        }

        // GET: api/notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.RecipientUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }

        // POST: api/notifications
        [HttpPost]
        public async Task<ActionResult> CreateNotification(int recipientUserId, string message)
        {
            var notification = new Notification
            {
                RecipientUserId = recipientUserId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/notifications/markAsRead
        [HttpPut("markAsRead")]
        public async Task<ActionResult> MarkNotificationsAsRead(List<int> notificationIds)
        {
            var notifications = await _context.Notifications
                .Where(n => notificationIds.Contains(n.Id))
                .ToListAsync();

            notifications.ForEach(n => n.IsRead = true);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}