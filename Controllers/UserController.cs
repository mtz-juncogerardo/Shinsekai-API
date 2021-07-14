using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public UserController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (dbUser == null)
            {
                return Ok(new ErrorResponse()
                {
                    Error = "Something went wrong"
                });
            }
            
            return Ok(new OkResponse()
            {
                Response = dbUser
            });
        }

        [Authorize]
        [HttpGet("cart")]
        public IActionResult GetCart()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbCart = _context.ShoppingCartArticles.Join(_context.Users,
                    s => s.UserId,
                    u => u.Id,
                    (s, u) => new
                    {
                        ShoppingCart = s,
                        User = u
                    }).Where(su => su.User.Id == id)
                .Select(su => su.ShoppingCart);

            return Ok(new
            {
                Result = dbCart
            });
        }

        [Authorize]
        [HttpGet("favorites")]
        public IActionResult GetFavorites()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbFavArticles = _context.Articles.Join(_context.Favorites,
                    a => a.Id,
                    f => f.ArticleId,
                    (a, f) => new
                    {
                        Favorites = f,
                        Article = a
                    }).Where(fa => fa.Favorites.UserId == id)
                .Select(fa => fa.Article);

            return Ok(new OkResponse()
            {
                Response = dbFavArticles
            });
        }

        [Authorize]
        [HttpGet("points")]
        public IActionResult GetPoints()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbPoints = _context.Points.Join(_context.Users,
                    p => p.UserId,
                    u => u.Id,
                    (p, u) => new
                    {
                        Points = p,
                        User = u
                    }).Where(pu => pu.User.Id == id)
                .Select(pu => pu.Points)
                .OrderBy(p => p.ExpirationDate).ToList();
            var expiredPoints = dbPoints.Where(p => p.ExpirationDate < DateTime.Now).Sum(p => p.Amount);
            var validPoints = dbPoints.Where(p => p.ExpirationDate >= DateTime.Now).Sum(p => p.Amount);

            return Ok(new OkResponse()
            {
                Response = new PointResponse()
                {
                    Points = dbPoints,
                    TotalExpired = expiredPoints,
                    TotalValid = validPoints
                }
            });
        }

        [Authorize]
        [HttpGet("purchases")]
        public IActionResult GetPurchases()
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbPurchases = _context.PurchasesArticles.Join(_context.Purchases,
                    pa => pa.PurchaseId,
                    p => p.Id,
                    (pa, p) => new
                    {
                        PurchaseArticle = pa,
                        Purchase = p
                    }).Join(_context.Articles,
                    pa => pa.PurchaseArticle.ArticleId,
                    a => a.Id,
                    (pa, a) => new
                    {
                        pa.PurchaseArticle,
                        Article = a
                    }).Where(paa => paa.PurchaseArticle.Purchase.UserId == id)
                .Select(paa => new
                {
                    paa.PurchaseArticle.PurchaseId,
                    paa.PurchaseArticle.Purchase.Date,
                    paa.PurchaseArticle.Article
                }).OrderBy(p => p.PurchaseId).AsEnumerable()
                .GroupBy(p => p.PurchaseId).ToList();

            return Ok(new OkResponse()
            {
                Response = dbPurchases,
                Count = dbPurchases.Count
            });
        }

        [Authorize]
        [HttpPost("favorites")]
        public IActionResult SaveFavorite([FromBody] ArticleItem article)
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbFavorite = _context.Favorites.FirstOrDefault(f => f.ArticleId == article.Id && f.UserId == id);

            if (dbFavorite != null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "La figura ya se encuentra en tus favoritos"
                });
            }

            var favorite = new FavoriteItem()
            {
                ArticleId = article.Id,
                UserId = id,
                Id = Guid.NewGuid().ToString()
            };

            _context.Favorites.Add(favorite);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = favorite
            });
        }

        [Authorize]
        [HttpPut]
        public IActionResult SaveFavorite([FromBody] UserItem user)
        {
            var id = AuthService.IdentifyUser(User.Identity);
            var dbUser = _context.Users.FirstOrDefault(u => u.Id == id);

            if (dbUser == null)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "El usuario no existe"
                });
            }

            if (user.Email == null)
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "Debes especificar un email"
                });
            }
            
            dbUser.Address = user.Address;
            dbUser.City = user.City;
            dbUser.PostalCode = user.PostalCode;
            dbUser.Email = user.Email;
            dbUser.Name = user.Name;
            dbUser.Phone = user.Phone;

            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = dbUser
            });
        }

    }
}