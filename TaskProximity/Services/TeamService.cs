using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskProximity.Data;
using TaskProximity.Models;

public class TeamService
{
    private readonly TaskProximityDbContext _context;

    public TeamService(TaskProximityDbContext context)
    {
        _context = context;
    }

    public List<Team> GetTeams()
    {
        return _context.Teams.ToList();
    }

    public Team GetTeam(int id)
    {
        return _context.Teams.Find(id);
    }

    public Team CreateTeam(Team team)
    {
        _context.Teams.Add(team);
        _context.SaveChanges();

        return team;
    }

    public void UpdateTeam(Team team)
    {
        _context.Entry(team).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void AddUserToTeam(int teamId, int userId)
    {
        var team = _context.Teams.Find(teamId);
        var user = _context.Users.Find(userId);

        if (team != null && user != null)
        {
            var userTeam = new UserTeam { UserId = userId, TeamId = teamId };
            _context.UserTeams.Add(userTeam);
            _context.SaveChanges();
        }
    }

    public void RemoveUserFromTeam(int teamId, int userId)
    {
        var userTeam = _context.UserTeams.FirstOrDefault(ut => ut.TeamId == teamId && ut.UserId == userId);

        if (userTeam != null)
        {
            _context.UserTeams.Remove(userTeam);
            _context.SaveChanges();
        }
    }
}