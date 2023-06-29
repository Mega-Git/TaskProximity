using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProximity.Data;
using TaskProximity.Models;

namespace TaskProximity.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TaskProximityDbContext _context;

        public ProjectService(TaskProximityDbContext context)
        {
            _context = context;
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            return project;
        }

        public async Task<List<int>> GetAssignedUserIds(int projectId)
        {
            // Retrieve assigned user IDs logic
            // ...
            var assignedUserIds = await _context.Projects
                .Where(p => p.Id == projectId)
                .SelectMany(p => p.UserProjects.Select(up => up.UserId))
                .ToListAsync();

            return assignedUserIds;
        }
    }
}