using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public AdminController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAdmins()
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context))
            {
                return Unauthorized(new
                {
                    Error = "You dont have the required role"
                });
            }

            return Ok(new
            {
                Result = "Thank you"
            });
        }
    }
}