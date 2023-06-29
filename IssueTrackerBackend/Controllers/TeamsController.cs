using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskProximity.Data;
using TaskProximity.Models;

namespace TaskProximity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly TaskProximityDbContext _context;

        public TeamsController(TaskProximityDbContext context)
        {
            _context = context;
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
    }
}