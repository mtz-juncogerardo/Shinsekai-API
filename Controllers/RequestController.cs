using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public RequestController(ShinsekaiApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var contactEmail = new UserRequestMail(req.Email, req.Name, req.PurchaseId, req.Message, req.Email, _configuration);
            contactEmail.SendEmail();
            
            return Ok(new OkResponse()
            {
                Response = "Se ha enviado la solicitud, te responderemos lo antes posible"
            });
        }
    }
}
