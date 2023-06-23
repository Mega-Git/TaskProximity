using TaskProximity.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProximity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Perform validation checks on the user object
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Save the user to the database or perform other registration-related operations

            // Return a success response
            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            // TODO: Implement user authentication logic
            // Compare the provided username/password with the stored values in the database
            // Generate and return a token upon successful authentication

            // Placeholder response for demonstration purposes
            return Ok(new { Message = "Login successful" });
        }
    }
}
