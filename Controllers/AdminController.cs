using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAdmins()
        {
            return Ok(new
            {
                Result = "Thank you"
            });
        }
    }
}