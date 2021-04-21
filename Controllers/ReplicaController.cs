using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("replica")]
    public class ReplicaController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public ReplicaController(ShinsekaiApiContext context)
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
