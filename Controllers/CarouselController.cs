using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("carousels")]
    public class CarouselController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public CarouselController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("read")]
        public IActionResult GetCarousels([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbCarousel = _context.Carousels.ToList();
            if (id == null)
            {
                return Ok(new OkResponse()
                {
                    Response = dbCarousel,
                    Count = dbCarousel.Count,
                    Page = 1,
                    MaxPage = 1
                });
            }

            var dbResponse = dbCarousel.Where(r => r.Id == id);
            return Ok(new OkResponse()
            {
                Response = dbResponse
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SaveCarousel([FromBody] CarouselItem carousel)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            carousel.Id = Guid.NewGuid().ToString();
            _context.Carousels.Add(carousel);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "New Carousel Published"
            });
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateCarousel([FromBody] CarouselItem carousel)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (carousel.Id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Id not specified"
                });
            }

            var entityExistsFlag = _context.Carousels.Any(q => q.Id == carousel.Id);

            if (!entityExistsFlag)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Carousel Invalid Id"
                });
            }

            _context.Update(carousel);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Carousel has been updated"
            });
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteCarousel([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una carousel para eliminar"
                });
            }

            var dbCarousel = _context.Carousels.FirstOrDefault(q => q.Id == id);

            if (dbCarousel == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbCarousel);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La carousel se elimino con exito"
            });
        }
    }
}