using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Authentication;
using Shinsekai_API.Config;
using Shinsekai_API.MailSender;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Shinsekai_API.Responses;
using Shinsekai_API.Services;
using Stripe;
using Stripe.Checkout;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchaseController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public PurchaseController(ShinsekaiApiContext context)
        {
            _context = context;
            StripeConfiguration.ApiKey = new ApiConfiguration().StripeKey;
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

            var response = new List<PurchaseResponse>();

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
                    }).Select(paapu => paapu)
                .OrderByDescending(p => p.PurchaseArticleArticlePurchase.Purchase.Date)
                .AsEnumerable()
                .GroupBy(p => p.PurchaseArticleArticlePurchase.Purchase.Id);

            foreach (var purchase in dbPurchases)
            {
                var articles = purchase.Select(r => new ArticleResponse(
                    r.PurchaseArticleArticlePurchase.PurchaseArticleArticle.Article,
                    r.PurchaseArticleArticlePurchase.PurchaseArticleArticle.PurchaseArticle.Quantity));

                response.Add(new PurchaseResponse()
                {
                    Id = purchase.Select(r => r.PurchaseArticleArticlePurchase.Purchase.Id).First(),
                    Articles = articles,
                    Buyer = purchase.Select(r => r.User).First(),
                    PurchaseDate = purchase.Select(r => r.PurchaseArticleArticlePurchase.Purchase.Date).First(),
                    Total = purchase.Select(r => r.PurchaseArticleArticlePurchase.Purchase.Total).First()
                });
            }

            if (id != null)
            {
                var purchaseResponse = response.FirstOrDefault(r => r.Id == id);
                return Ok(new OkResponse()
                {
                    Response = purchaseResponse
                });
            }

            return Ok(new OkResponse()
            {
                Response = response
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

                paymentService = new PaymentService(payment, points.Sum(p => p.Amount));
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
            Session session = service.Create(options);
            var response = service.Get(session.Id);
            return Ok(new OkResponse()
            {
                Response = new
                {
                    SessionUrl = response.Url,
                    Id = response.Id,
                    Articles = payment.Articles,
                    CashPoints = paymentService.GetCashPoints(),
                    TotalPrice = paymentService.GetTotal()
                }
            });
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> SavePurchase([FromBody] PurchaseItem purchase)
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var anonBuyer = false;
            var purchasesArticles = purchase.PurchasesArticles;
            if (!purchasesArticles.Any())
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "The purchase has no articles defined"
                });
            }

            var buyer = _context.Users.FirstOrDefault(u => u.Id == id);

            if (buyer == null)
            {
                anonBuyer = true;
                var userClaim = (ClaimsIdentity)User.Identity ?? new ClaimsIdentity();
                buyer = new UserItem()
                {
                    Id = id,
                    Address = userClaim.Claims.Where(r => r.Type == ClaimTypes.Locality)
                        .Select(c => c.Value)
                        .First(),
                    City = userClaim.Claims.Where(r => r.Type == ClaimTypes.Country)
                        .Select(c => c.Value)
                        .First(),
                    Email = userClaim.Claims.Where(r => r.Type == ClaimTypes.Email)
                        .Select(c => c.Value)
                        .First(),
                    Phone = userClaim.Claims.Where(r => r.Type == ClaimTypes.HomePhone)
                        .Select(c => c.Value)
                        .First(),
                    PostalCode = userClaim.Claims.Where(r => r.Type == ClaimTypes.PostalCode)
                        .Select(c => c.Value)
                        .First(),
                    Name = userClaim.Claims.Where(r => r.Type == ClaimTypes.Name)
                        .Select(c => c.Value)
                        .First(),
                };

                var registered = _context.Users.FirstOrDefault(u => u.Email == buyer.Email);

                if (registered == null)
                {
                    _context.Users.Add(buyer);
                }
                else
                {
                    buyer.Id = registered.Id;
                }
            }
            else if (buyer.Address == null || buyer.City == null || buyer.Name == null || buyer.Phone == null || buyer.PostalCode == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Se debe especificar una dirección, una ciudad, un codigo postal, un nombre y un telefono para autorizar la compra."
                });
            }

            purchase.Id = Guid.NewGuid().ToString();
            purchase.UserId = buyer.Id;
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

            var simplePurchase = purchase.PurchasesArticles.Select(p => new
            {
                Price = p.Article.Price,
                DiscountPrice = p.Article.DiscountPrice <= 0.1M ? 0 : p.Article.DiscountPrice,
                Quantity = p.Quantity
            }).ToList();

            purchase.Total = simplePurchase.Sum(r => (r.Price - r.DiscountPrice) * r.Quantity);

            if (purchase.CashPoints > 0)
            {
                // var totalPrice = simplePurchase.Sum(a => (a.Price - a.DiscountPrice) * a.Quantity);
                // var totalQuantity = simplePurchase.Sum(a => a.Quantity);
                // var totalWithDiscount = totalPrice - purchase.CashPoints;
                // var cashPoints = totalWithDiscount < 0 ? totalWithDiscount : 0;
                //
                // purchase.Total = 0;
                // foreach (var article in purchase.PurchasesArticles)
                // {
                //     purchase.Total += cashPoints == 0 ? totalWithDiscount / totalQuantity : decimal.Zero;
                // }

                var remainingPoints = (purchase.CashPoints - purchase.Total);
                purchase.Total -= purchase.CashPoints;

                var dbPoints = _context.Points.Where(p => p.UserId == id && p.ExpirationDate > DateTime.Now)
                    .OrderBy(p => p.ExpirationDate)
                    .ToList();

                foreach (var point in dbPoints)
                {
                    _context.Points.Remove(point);
                }

                if (remainingPoints > 0)
                {
                    var pointItem = new PointItem()
                    {
                        ExpirationDate = dbPoints.First().ExpirationDate,
                        UserId = id,
                        Amount = remainingPoints,
                        Id = Guid.NewGuid().ToString()
                    };
                    _context.Points.Add(pointItem);
                }
            }

            if (!anonBuyer)
            {
                var pointItem = new PointItem()
                {
                    ExpirationDate = DateTime.Now.AddYears(1),
                    UserId = id,
                    Amount = purchase.Total / 100,
                    Id = Guid.NewGuid().ToString()
                };
                _context.Points.Add(pointItem);
            }

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            var emailService = new PurchaseConfirmationMail(buyer.Email, purchase.Id);
            await emailService.SendEmail();
            var emailRequest = new PurchaseRequestMail(purchase);
            await emailRequest.SendEmail();

            return Ok(new OkResponse()
            {
                Response = new
                {
                    Id = purchase.Id,
                    Name = buyer.Name,
                    Address = buyer.Address,
                    PostalCode = buyer.PostalCode,
                    City = buyer.City,
                    TotalAmount = purchase.Total
                }
            });
        }

        [HttpGet("validate")]
        public IActionResult GetPaymentValidation([FromQuery] string paymentId)
        {
            var service = new SessionService();
            var response = service.Get(paymentId);

            return Ok(new OkResponse()
            {
                Response = response
            });
        }
    }
}