using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProximity.Data;
using TaskProximity.Models;
using TaskProximity.Services;

namespace TaskProximity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly TaskProximityDbContext _context;
        private readonly TeamService _teamService;
        private readonly IEmailNotificationService _emailNotificationService;

        public TeamsController(TaskProximityDbContext context, TeamService teamService, IEmailNotificationService emailNotificationService)
        {
            _context = context;
            _teamService = teamService;
            _emailNotificationService = emailNotificationService;
        }

        // GET: api/teams
        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetTeams()
        {
            return _context.Teams.ToList();
        }

        // GET: api/teams/5
        [HttpGet("{id}")]
        public ActionResult<Team> GetTeam(int id)
        {
            var team = _context.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // POST: api/teams
        [HttpPost]
        public ActionResult<Team> CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }

        // PUT: api/teams/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")] // Only managers can access this action
        public IActionResult UpdateTeam(int id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // POST: api/teams/{teamId}/users/{userId}
        [HttpPost("{teamId}/users/{userId}")]
        [Authorize(Roles = "Manager")] // Only managers can access this action
        public IActionResult AddUserToTeam(int teamId, int userId)
        {
            var team = _context.Teams.Find(teamId);
            var user = _context.Users.Find(userId);

            if (team == null || user == null)
            {
                return NotFound();
            }

            var userTeam = new UserTeam { UserId = userId, TeamId = teamId };
            _context.UserTeams.Add(userTeam);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/teams/{teamId}/users/{userId}
        [HttpDelete("{teamId}/users/{userId}")]
        [Authorize(Roles = "Manager")] // Only managers can access this action
        public IActionResult RemoveUserFromTeam(int teamId, int userId)
        {
            var userTeam = _context.UserTeams.FirstOrDefault(ut => ut.TeamId == teamId && ut.UserId == userId);

            if (userTeam == null)
            {
                return NotFound();
            }

            _context.UserTeams.Remove(userTeam);
            _context.SaveChanges();

            return Ok();
        }

        // POST: api/teams/{teamId}/invite
        [HttpPost("{teamId}/invite")]
        public async Task<ActionResult> InviteUserToTeam(int teamId, [FromBody] string email)
        {
            var team = await _context.Teams.FindAsync(teamId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (team == null || user == null)
            {
                return NotFound();
            }

            if (await IsUserInTeam(user.Id, teamId))
            {
                return BadRequest("User is already a member of the team.");
            }

            var invitation = new Invitation
            {
                TeamId = teamId,
                UserId = user.Id
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();

            // Send email notification to the invited user
            var subject = "Invitation to join a team";
            var body = $"You have been invited to join the team: {team.Name}.";
            await _emailNotificationService.SendEmailNotification(email, subject, body);

            return Ok();
        }

        // DELETE: api/teams/{teamId}/invitations/{invitationId}
        [HttpDelete("{teamId}/invitations/{invitationId}")]
        public async Task<ActionResult> CancelInvitation(int teamId, int invitationId)
        {
            var invitation = await _context.Invitations.FindAsync(invitationId);

            if (invitation == null || invitation.TeamId != teamId)
            {
                return NotFound();
            }

            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/teams/{teamId}/accept/{invitationId}
        [HttpPost("{teamId}/accept/{invitationId}")]
        public async Task<ActionResult> AcceptInvitation(int teamId, int invitationId)
        {
            var invitation = await _context.Invitations.FindAsync(invitationId);

            if (invitation == null || invitation.TeamId != teamId)
            {
                return NotFound();
            }

            var userTeam = new UserTeam
            {
                UserId = invitation.UserId,
                TeamId = invitation.TeamId,
                Role = Role.Member // Assign a role to the user when accepting the invitation
            };

            _context.UserTeams.Add(userTeam);
            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private async Task<bool> IsUserInTeam(int userId, int teamId)
        {
            return await _context.UserTeams.AnyAsync(ut => ut.UserId == userId && ut.TeamId == teamId);
        }
    }
}