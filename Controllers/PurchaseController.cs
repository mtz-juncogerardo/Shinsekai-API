using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
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

        [Authorize]
        [HttpPost("checkout")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest payment)
        {
            PaymentService paymentService;

            if (payment.PayWithPoints)
            {
                var id = AuthService.IdentifyUser(User.Identity);
                var points = _context.Points.Where(p => p.UserId == id && p.ExpirationDate > DateTime.Now)
                    .ToList();

                paymentService = new PaymentService(payment, (int)points.Sum(p => p.Amount));
            }
            else
            {
                paymentService = new PaymentService(payment);
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = paymentService.CreateLineItems(),
                Mode = "payment",
                SuccessUrl = paymentService.SuccessUrl,
                CancelUrl = paymentService.ErrorUrl
            };
            var service = new SessionService();
            var session = service.Create(options);
            return Ok(new OkResponse()
            {
                Response = new
                {
                    session.Id,
                    payment.PayWithPoints,
                    CashPoints = paymentService.GetCashPoints(),
                    TotalPrice = paymentService.GetTotal()
                }
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SavePurchase([FromBody] PurchaseItem purchase)
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var purchasesArticles = purchase.PurchasesArticles;
            if (!purchasesArticles.Any())
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "The purchase has no articles defined"
                });
            }

            var buyer = _context.Users.FirstOrDefault(u => u.Id == purchase.UserId);

            if (buyer == null)
            {
                var userClaim = (ClaimsIdentity)User.Identity ?? new ClaimsIdentity();
                buyer = new UserItem()
                {
                    Id = id,
                    Address = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                        .Select(c => c.Value)
                        .First(),
                    City = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                        .Select(c => c.Value)
                        .First(),
                    Email = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                        .Select(c => c.Value)
                        .First(),
                    Phone = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                        .Select(c => c.Value)
                        .First(),
                };

                _context.Users.Add(buyer);
            }
            else if (buyer.Address == null || buyer.City == null || buyer.Name == null || buyer.Phone == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "You must specify an Adress, a City, a Name and a Phone so we can authorize to buy"
                });
            }

            purchase.Id = Guid.NewGuid().ToString();
            purchase.UserId = id;
            purchase.Date = DateTime.Now;
            purchase.PurchasesArticles = new List<PurchaseArticleItem>();

            foreach (var purchaseArticle in purchasesArticles)
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
                    ArticleId = articleId,
                    SoldDate = DateTime.Now
                };
                purchaseArticle.Article = dbArticle;
                purchaseArticle.Id = Guid.NewGuid().ToString();
                purchaseArticle.PurchaseId = purchase.Id;
                dbArticle.Stock--;

                purchase.PurchasesArticles.Add(purchaseArticle);

                _context.Sales.Add(sale);
                _context.Articles.Update(dbArticle);
            }

            purchase.Total = purchase.PurchasesArticles.Sum(p => (p.Article.Price - p.Article.DiscountPrice) * p.Quantity);

            if (purchase.CashPoints > 0)
            {
                var remainingPoints = purchase.CashPoints;
                var dbPoints = _context.Points.Where(p => p.UserId == id && p.ExpirationDate > DateTime.Now)
                    .OrderBy(p => p.ExpirationDate)
                    .ToList();

                foreach (var point in dbPoints)
                {
                    if (remainingPoints == 0) break;
                    var total = (int)point.Amount - purchase.CashPoints;

                    point.Amount = total < 0 ? decimal.Zero : total;
                    remainingPoints = total >= 0 ? 0 : Math.Abs(total);

                    if (point.Amount == 0)
                    {
                        _context.Points.Remove(point);
                        break;
                    }

                    _context.Points.Update(point);
                }
            }

            var pointItem = new PointItem()
            {
                ExpirationDate = DateTime.Now.AddYears(1),
                UserId = id,
                Amount = purchase.Total / 100,
                Id = Guid.NewGuid().ToString()
            };

            _context.Points.Add(pointItem);
            _context.SaveChanges();

            var emailService = new PurchaseConfirmationMail(buyer.Email, purchase.Id);
            emailService.SendEmail();

            return Ok(new OkResponse()
            {
                Response = "Purchase has been saved, An email was sent to the buyer"
            });
        }
    }
}