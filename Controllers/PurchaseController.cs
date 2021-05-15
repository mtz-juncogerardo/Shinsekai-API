using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.MailSender;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Shinsekai_API.Responses;
using Shinsekai_API.Services;
using Stripe.Checkout;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public PurchaseController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpPost("checkout")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest payment)
        {
            var paymentService = new PaymentService(payment);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = paymentService.LineItems,
                Mode = "payment",
                SuccessUrl = paymentService.SuccessUrl,
                CancelUrl = paymentService.ErrorUrl,
            };
            var service = new SessionService();
            var session = service.Create(options);
            return Ok(new OkResponse()
            {
                Response = session.Id
            });
        }

        [HttpPost("create")]
        public IActionResult SavePurchase([FromBody] PurchaseItem purchase)
        {
            if (purchase.BuyerEmail == null && purchase.UserId == null)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "Who is the buyer?"
                });
            }

            if (!purchase.PurchasesArticles.Any())
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "The purchase has no articles defined"
                });
            }

            purchase.BuyerEmail ??= _context.Users.FirstOrDefault(u => u.Id == purchase.UserId)?.Email;
            purchase.Id = Guid.NewGuid().ToString();

            foreach (var purchaseArticle in purchase.PurchasesArticles)
            {
                var articleId = purchaseArticle.ArticleId ?? purchaseArticle.Article.Id;
                var dbArticle = _context.Articles.FirstOrDefault(a => a.Id == articleId);
                if (dbArticle is not {Stock: > 0})
                {
                    return BadRequest(new ErrorResponse() 
                    {
                        Error = "Some articles arent in stock anymore or doesnt exist"
                    });
                }
                
                var sale = new SaleItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleId = purchaseArticle.ArticleId ?? purchaseArticle.Article.Id,
                    SoldDate = DateTime.Now
                };
                purchaseArticle.ArticleId = articleId;
                purchaseArticle.Id = Guid.NewGuid().ToString();
                purchaseArticle.Purchase = purchase;
                dbArticle.Stock--;

                _context.Sales.Add(sale);
                _context.Articles.Update(dbArticle);
                _context.PurchasesArticles.Add(purchaseArticle);
            }

            _context.SaveChanges();

            var emailService = new PurchaseConfirmationMail(purchase.BuyerEmail, purchase.Id);
            emailService.SendEmail();

            return Ok(new OkResponse()
            {
                Response = "Purchase has been saved, An email was sent to the buyer"
            });
        }
    }
}