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

        [HttpGet]
        public IActionResult GetDeliveries()
        {
            var dbDeliveries = _context.Deliveries.ToList();

            return Ok(new OkResponse()
            {
                Response = dbDeliveries
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SaveDelivery(DeliveryItem delivery)
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
                Response = "Delivery Added succesfully"
            });
        }

        [Authorize]
        [HttpGet("read")]
        public IActionResult GetDeliveryById([FromQuery] string id)
        {
            if (id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico una entrega"
                });
            }

            var dbDelivery = _context.Deliveries.FirstOrDefault(d => d.Id == id);

            if (dbDelivery == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Error = "La entregaa no existe"
                });
            }

            return Ok(new OkResponse()
            {
                Response = dbDelivery
            });
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateDelivery([FromBody] DeliveryRequest delivery)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbDelivery = _context.Deliveries.FirstOrDefault(d => d.Id == delivery.Id);

            if (dbDelivery is null || delivery is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "La entrega a editar ya no existe"
                });
            }

            dbDelivery.Parcel = delivery.Parcel ?? dbDelivery.Parcel;
            dbDelivery.LocationId = delivery.LocationId ?? dbDelivery.LocationId;
            dbDelivery.EstimatedDays = delivery.EstimatedDays == 0 ? dbDelivery.EstimatedDays : delivery.EstimatedDays;
            
            _context.Update(dbDelivery);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La entrega se actualizo con exito"
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
                    Error = "No se especifico una entrega para eliminar"
                });
            }

            var dbDelivery = _context.Deliveries.FirstOrDefault(d => d.Id == id);

            if (dbDelivery == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Error = "La Entrega No existe"
                });
            }

            _context.Remove(dbDelivery);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "La Entrega se elimino con exito"
            });
        }

    }
}