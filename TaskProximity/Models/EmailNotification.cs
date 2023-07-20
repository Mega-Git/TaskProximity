using System;

namespace TaskProximity.Models
{
    public class EmailNotification
    {
        public int Id { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}