using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TaskProximity.Data;
using TaskProximity.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace TaskProximity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TaskProximityDbContext _context;

        public UsersController(TaskProximityDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Perform validation checks on the user object
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username or email already exists
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return BadRequest(ModelState);
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return BadRequest(ModelState);
            }

            // Hash the password
            string hashedPassword = HashPassword(user.Password);

            // Create a new User object with the hashed password
            var newUser = new User
            {
                Username = user.Username,
                Password = hashedPassword,
                Email = user.Email,
                Role = user.Role
            };

            // Save the new user to the database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Return a success response
            return Ok(new { Message = "Registration successful" });
        }

        private string HashPassword(string password)
        {
            // Generate a random salt for the password
            string salt = BCryptNet.GenerateSalt();

            // Hash the password using BCrypt with the generated salt
            string hashedPassword = BCryptNet.HashPassword(password, salt);

            return hashedPassword;
        }
    }
}