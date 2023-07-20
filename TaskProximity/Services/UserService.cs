using System.Threading.Tasks;
using TaskProximity.Data;
using TaskProximity.Models;

namespace TaskProximity.Services
{
    public class UserService : IUserService
    {
        private readonly TaskProximityDbContext _context;

        public UserService(TaskProximityDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user;
        }
    }
}