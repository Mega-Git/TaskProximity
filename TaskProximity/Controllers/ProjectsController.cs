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
    public class ProjectsController : ControllerBase
    {
        private readonly TaskProximityDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IProjectService _projectService;

        public ProjectsController(TaskProximityDbContext context, INotificationService notificationService, IUserService userService, IEmailNotificationService emailNotificationService, IProjectService projectService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _emailNotificationService = emailNotificationService;
            _projectService = projectService;
        }

        // GET: api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // POST: api/projects
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            // Set the "CreatedBy" property with the authenticated user's information
            project.CreatedBy = User.Identity.Name;

            // Set the initial status of the project
            project.Status = "In Progress";

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Get the list of user IDs assigned to the project
            var assignedUserIds = await _projectService.GetAssignedUserIds(project.Id);

            // Trigger in-app notifications
            var message = $"The project {project.Id} has been updated.";
            foreach (var userId in assignedUserIds)
            {
                await _notificationService.CreateNotification(userId, message);
            }

            // Trigger email notifications
            var subject = "Project Update";
            var body = $"The project {project.Name} has been updated.";
            foreach (var userId in assignedUserIds)
            {
                var user = await _userService.GetUserById(userId);
                await _emailNotificationService.SendEmailNotification(user.Email, subject, body);
            }

            return Ok();
        }

        // DELETE: api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.Id == id);
        }

        // POST: api/projects/{projectId}/users/{userId}
        [HttpPost("{projectId}/users/{userId}")]
        public async Task<ActionResult> AssignUserToProject(int projectId, int userId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
            {
                return NotFound();
            }

            project.UserProjects.Add(new UserProject
            {
                UserId = userId,
                ProjectId = projectId
            });

            await _context.SaveChangesAsync();
            // Trigger in-app notification
            var message = $"You have been assigned to the project: {projectId}";
            await _notificationService.CreateNotification(userId, message);

            // Trigger email notification
            user = await _userService.GetUserById(userId);
            project = await _projectService.GetProjectById(projectId);
            var subject = "Project Assignment";
            var body = $"You have been assigned to the project: {project.Name}";
            await _emailNotificationService.SendEmailNotification(user.Email, subject, body);

            return Ok();
        }

        // DELETE: api/projects/{projectId}/users/{userId}
        [HttpDelete("{projectId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromProject(int projectId, int userId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            var user = await _context.Users.FindAsync(userId);

            if (project == null || user == null)
            {
                return NotFound();
            }

            var userProject = project.UserProjects.FirstOrDefault(up => up.UserId == userId && up.ProjectId == projectId);

            if (userProject != null)
            {
                project.UserProjects.Remove(userProject);
                await _context.SaveChangesAsync();
            }

            // Trigger in-app notification
            var message = $"You have been unassigned from the project: {projectId}";
            await _notificationService.CreateNotification(userId, message);

            // Trigger email notification
            user = await _userService.GetUserById(userId);
            project = await _projectService.GetProjectById(projectId);
            var subject = "Project Unassignment";
            var body = $"You have been unassigned from the project: {project.Name}";
            await _emailNotificationService.SendEmailNotification(user.Email, subject, body);

            return Ok();
        }
    }
}