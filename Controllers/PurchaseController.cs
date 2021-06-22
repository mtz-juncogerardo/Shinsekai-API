using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    [Route("api/purchases")]
    public class PurchaseController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        private readonly IConfiguration _configuration;

        public PurchaseController(ShinsekaiApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("read")]
        public IActionResult GetPurchasesForAdmin([FromQuery] string id)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbPurchases = _context.PurchasesArticles.Join(_context.Articles,
                    pa => pa.ArticleId,
                    a => a.Id,
                    (pa, a) => new
                    {
                        PurchaseArticle = pa,
                        Article = a
                    }).Join(_context.Purchases,
                    paa => paa.PurchaseArticle.PurchaseId,
                    p => p.Id,
                    (paa, p) => new
                    {
                        PurchaseArticleArticle = paa,
                        Purchase = p
                    }).Join(_context.Users,
                    paap => paap.Purchase.UserId,
                    u => u.Id,
                    (paap, u) => new
                    {
                        PurchaseArticleArticlePurchase = paap,
                        User = u
                    }).Select(paapu => paapu.PurchaseArticleArticlePurchase.Purchase)
                .AsEnumerable()
                .GroupBy(p => p.UserId);

            return Ok(new OkResponse()
            {
                Response = dbPurchases
            });
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

            var buyer = _context.Users.FirstOrDefault(u => u.Id == purchase.Id);

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
                    Error = "Se debe especificar una dirección, una ciudad, un nombre y un telefono para autorizar la compra."
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
                if (dbArticle == null)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        Error = "Estas comprando un articulo que no existe"
                    });
                }

                if (dbArticle is {Stock: > 0} && dbArticle.Stock < purchaseArticle.Quantity)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        Error = "Uno de los articulos que intentas comprar ya no se encuentra disponible."
                    });
                }

                for (var i = 0; i < purchaseArticle.Quantity; i++)
                {
                    var sale = new SaleItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ArticleId = articleId,
                        SoldDate = DateTime.Now
                    };

                    _context.Sales.Add(sale);
                }

                purchaseArticle.Article = dbArticle;
                purchaseArticle.Id = Guid.NewGuid().ToString();
                purchaseArticle.PurchaseId = purchase.Id;
                dbArticle.Stock -= purchaseArticle.Quantity;

                purchase.PurchasesArticles.Add(purchaseArticle);

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

            _context.Purchases.Add(purchase);
            _context.Points.Add(pointItem);
            _context.SaveChanges();

            var emailService = new PurchaseConfirmationMail(buyer.Email, purchase.Id, _configuration);
            emailService.SendEmail();

            return Ok(new OkResponse()
            {
                Response = "Purchase has been saved, An email was sent to the buyer"
            });
        }
    }
}