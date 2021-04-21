using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        
        public ArticleController(ShinsekaiApiContext context)
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
