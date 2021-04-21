using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("request")]
    public class RequestController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public RequestController(ShinsekaiApiContext context)
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
