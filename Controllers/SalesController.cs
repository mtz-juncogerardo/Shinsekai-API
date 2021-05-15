using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("sales")]
    public class SalesController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public SalesController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSales([FromQuery] string search, string articleId, bool orderByDate)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            var dbSale = _context.Sales.ToList();

            if (articleId != null)
            {
                dbSale = dbSale.Where(s => s.ArticleId == articleId).ToList();
            }

            if (search != null)
            {
                var dbSalesArticles = dbSale.Join(_context.Articles,
                    s => s.ArticleId,
                    a => a.Id,
                    (s, a) => new
                    {
                        Sale = s,
                        Article = a
                    });
                
                dbSale = dbSalesArticles.Where(sa => sa.Article.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .Select(sa => sa.Sale).ToList();
            }

            if (orderByDate)
            {
                dbSale = dbSale.OrderByDescending(s => s.SoldDate).ToList();
            }
            
            return Ok(new OkResponse()
            {
                Response = dbSale,
                Count = dbSale.Count,
                Page = 1,
                MaxPage = 1
            });
        }
    }
}