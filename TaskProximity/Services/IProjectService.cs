using System.Collections.Generic;
using System.Threading.Tasks;
using TaskProximity.Models;

namespace TaskProximity.Services
{
    public interface IProjectService
    {
        Task<Project> GetProjectById(int projectId);

        Task<List<int>> GetAssignedUserIds(int projectId);
    }
}