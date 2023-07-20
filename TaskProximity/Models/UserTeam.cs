using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProximity.Models
{
    public class UserTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTeamId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int RoleId { get; set; }
        public string Role { get; set; }

        public int InvitationId { get; set; }
        public Invitation Invitation { get; set; }
    }
}