namespace TaskProximity.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int InvitedUserId { get; set; }
        public bool IsAccepted { get; set; }

        public Team Team { get; set; }
        public User InvitedUser { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}