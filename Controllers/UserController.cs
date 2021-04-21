using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public UserController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUserByEmail([FromQuery] string email)
        {
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (foundUser == null)
            {
                return NotFound(new
                {
                    Error = "User not found on records"
                });
            }
            return Ok(new
            {
                Result = foundUser 
            });
        }
    }
}