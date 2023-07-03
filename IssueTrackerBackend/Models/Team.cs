using System.Collections.Generic;

namespace TaskProximity.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<UserTeam> UserTeams { get; set; }
        public List<Invitation> Invitations { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}