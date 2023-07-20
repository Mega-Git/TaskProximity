using System.Threading.Tasks;
using TaskProximity.Models;

namespace TaskProximity.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
    }
}