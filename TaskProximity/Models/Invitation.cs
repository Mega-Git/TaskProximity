using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProximity.Models
{
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvitationId { get; set; }

        [ForeignKey("InvitedUserId")]
        public int InvitedUserId { get; set; }
        public User InvitedUser { get; set; }

        [ForeignKey("TeamId")]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public bool IsAccepted { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}