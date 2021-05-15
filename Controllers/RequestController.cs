using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.MailSender;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Shinsekai_API.Responses;

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

        [HttpPost]
        public IActionResult SendEmailRequest([FromBody] ContactRequest req)
        {
            var dbPurchase = _context.Purchases.FirstOrDefault(p => p.Id == req.PurchaseId);

            if (dbPurchase == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Error = $"No hay ninguna compra registrada con el Id: {req.PurchaseId}"
                });
            }

            var contactEmail = new UserRequestMail(req.Email, req.Name, req.PurchaseId, req.Message);
            contactEmail.SendEmail();
            
            return Ok(new OkResponse()
            {
                Response = "Se ha enviado la solicitud, te responderemos lo antes posible"
            });
        }
    }
}
