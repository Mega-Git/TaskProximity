using System;

namespace TaskProximity.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int RecipientUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User RecipientUser { get; set; }
    }
}