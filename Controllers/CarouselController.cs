using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

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
        public IActionResult GetCarrousel()
        {
            var id = AuthService.IdentifyUser(User.Identity);

            var dbCarousels = _context.Carousels.ToList();
            
            return Ok(new
            {
                Result =  dbCarousels
            });
        }

        [Authorize]
        [HttpPost("carousel")]
        public IActionResult SaveCarousel([FromBody] CarouselItem carousel)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            _context.Carousels.Add(carousel);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "El nuevo elemento del carrusel fue añadido con exito"
            });
        }

    }
}
