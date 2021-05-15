using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/delivery")]
    public class DeliveryController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public DeliveryController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("read")]
        public IActionResult GetDeliveries([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbDelivery = _context.Deliveries.ToList();
            if (id == null)
            {
                return Ok(new OkResponse()
                {
                    Response = dbDelivery,
                    Count = dbDelivery.Count,
                    Page = 1,
                    MaxPage = 1
                });
            }

            var dbResponse = dbDelivery.Where(r => r.Id == id);
            return Ok(new OkResponse()
            {
                Response = dbResponse
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SaveDelivery([FromBody] DeliveryItem delivery)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            delivery.Id = Guid.NewGuid().ToString();
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "New Delivery Published"
            });
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateDelivery([FromBody] DeliveryItem delivery)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (delivery.Id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Id not specified"
                });
            }

            var entityExistsFlag = _context.Deliveries.Any(q => q.Id == delivery.Id);

            if (!entityExistsFlag)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Delivery Invalid Id"
                });
            }

            _context.Update(delivery);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Delivery has been updated"
            });
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteDelivery([FromQuery] string id)
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
                    Error = "No se especifico una delivery para eliminar"
                });
            }

            var dbDelivery = _context.Deliveries.FirstOrDefault(q => q.Id == id);

            if (dbDelivery == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Delivery que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbDelivery);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La delivery se elimino con exito"
            });
        }
    }
}