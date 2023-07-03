namespace TaskProximity.Models
{
    public class UserTeam
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; }
        public Team Team { get; set; }
        public string Role { get; set; }

        public int InvitationId { get; set; }
        public Invitation Invitation { get; set; }
    }
}