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
    [Route("api/promotions")]
    public class PromotionController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public PromotionController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet("read")]
        public IActionResult GetPromotions([FromQuery] string id)
        {
            var dbPromotion = _context.Promotions.ToList();
            if (id == null)
            {
                return Ok(new OkResponse()
                {
                    Response = dbPromotion,
                    Count = dbPromotion.Count,
                    Page = 1,
                    MaxPage = 1
                });
            }

            var dbResponse = dbPromotion.Where(r => r.Id == id);
            return Ok(new OkResponse()
            {
                Response = dbResponse
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SavePromotion([FromBody] PromotionItem promotion)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            promotion.Id = Guid.NewGuid().ToString();
            _context.Promotions.Add(promotion);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = promotion
            });
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdatePromotion([FromBody] PromotionItem promotion)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (promotion.Id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Id not specified"
                });
            }

            var entityExistsFlag = _context.Promotions.Any(q => q.Id == promotion.Id);

            if (!entityExistsFlag)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Promotion Invalid Id"
                });
            }

            _context.Update(promotion);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = promotion
            });
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeletePromotion([FromQuery] string id)
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
                    Error = "No se especifico una promotion para eliminar"
                });
            }

            var dbPromotion = _context.Promotions.FirstOrDefault(q => q.Id == id);

            if (dbPromotion == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Promotion que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbPromotion);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La promotion se elimino con exito"
            });
        }
    }
}