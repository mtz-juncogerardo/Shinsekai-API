using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("carousel")]
    public class CarouselController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public CarouselController(ShinsekaiApiContext context)
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
