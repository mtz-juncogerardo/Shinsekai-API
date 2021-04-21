using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("tag")]
    public class TagController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public TagController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetArticles()
        {
            return Ok(new
            {
                Result = "Thank you"
            });
        }
    }
}
