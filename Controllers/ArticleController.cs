using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Authentication;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Shinsekai_API.Responses;
using Shinsekai_API.Services;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;
        private const int _numberOfElementsInPage = 20;

        public ArticleController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetArticles([FromQuery] string page,
            string search,
            string animeId,
            string brandId,
            string lineId,
            string materialId,
            bool orderBySales)
        {
            var pageNum = page == null ? 1 : int.Parse(page);
            var elementsToSkip = (pageNum - 1) * _numberOfElementsInPage;
            var dbArticles = _context.Articles.ToList();

            if (brandId != null)
            {
                dbArticles = _context.Articles.Where(a => a.BrandId == brandId).ToList();
            }

            if (animeId != null)
            {
                dbArticles = _context.AnimesArticles.Where(aa => aa.AnimeId == animeId)
                    .Join(_context.Animes,
                        aa => aa.AnimeId,
                        a => a.Id,
                        (aa, a) => new
                        {
                            AnimeArticle = aa,
                            Anime = a
                        }).Join(_context.Articles,
                        aaa => aaa.AnimeArticle.ArticleId,
                        a => a.Id,
                        (aaa, a) => new
                        {
                            AnimeArticleAnime = aaa,
                            Article = a
                        }).Select(a => a.Article).ToList();
            }

            if (materialId != null)
            {
                dbArticles = _context.MaterialsArticles.Where(ma => ma.MaterialId == materialId)
                    .Join(_context.Materials,
                        m => m.MaterialId,
                        a => a.Id,
                        (m, a) => new
                        {
                            MaterialArticle = m,
                            Material = a
                        }).Join(_context.Articles,
                        ma => ma.MaterialArticle.ArticleId,
                        a => a.Id,
                        (ma, a) => new
                        {
                            MaterialArticle = ma,
                            Article = a
                        }).Select(a => a.Article).ToList();
            }

            if (lineId != null)
            {
                dbArticles = _context.LinesArticles.Where(la => la.LineId == lineId)
                    .Join(_context.Lines,
                        la => la.LineId,
                        l => l.Id,
                        (la, l) => new
                        {
                            LineArticle = la,
                            Line = l
                        }).Join(_context.Articles,
                        la => la.LineArticle.ArticleId,
                        a => a.Id,
                        (la, a) => new
                        {
                            LineArticle = la,
                            Article = a
                        }).Select(a => a.Article).ToList();
            }

            if (search != null)
            {
                dbArticles = dbArticles.Where(a => a.Name.Trim().ToLower()
                        .Contains(search.Trim().ToLower()))
                    .ToList();
            }


            if (orderBySales)
            {
                dbArticles = dbArticles.Select(a => new
                    {
                        Article = a,
                        TotalSales = _context.Sales.Count(s => s.ArticleId == a.Id)
                    }).OrderByDescending(r => r.TotalSales)
                    .Select(a => a.Article).ToList();
            }
            
            dbArticles.ForEach(a => a.Brand = _context.Brands.FirstOrDefault(b => b.Id == a.BrandId));
            var articleCount = dbArticles.Count;
            var maxPages = (int)Math.Ceiling(articleCount / (decimal)_numberOfElementsInPage);
            var responseArticles = dbArticles.Skip(elementsToSkip).Take(_numberOfElementsInPage);

            return Ok(new OkResponse()
            {
                Response = responseArticles,
                Count = responseArticles.Count(),
                Page = pageNum,
                MaxPage = maxPages
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult SaveArticle([FromBody] ArticleRequest article)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (article.BrandId == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Necesitas especificar una marca"
                });
            }

            if (article.Images == null || !article.Images.Any())
            {
                return BadRequest(new ErrorResponse() 
                {
                    Error = "Tu articulo necesita al menos una imagen"
                });
            }

            article.Id = Guid.NewGuid().ToString();
            
            foreach (var image in article.Images)
            {
                image.Id = Guid.NewGuid().ToString();
            }

            if (article.OriginalFlag)
            {
                var original = new OriginalItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Article = article
                };
                _context.Originals.Add(original);
            }
            else
            {
                _context.Articles.Add(article);
            }


            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "Article Published"
            });
        }

        [HttpGet("read")]
        public IActionResult GetArticleById([FromQuery] string id)
        {
            var dbArticle = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (dbArticle == null)
            {
                return NotFound(new ErrorResponse()
                {
                    Error = "El articulo que intentabas ver ya no existe"
                });
            }

            return Ok(new OkResponse()
            {
                Response = dbArticle,
                Count = 1,
                Page = 1,
                MaxPage = 1
            });
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateItem([FromBody] ArticleItem article)
        {
            if (AuthService.AuthorizeAdmin(User.Identity, _context.Users.ToList()))
            {
                return Unauthorized(new ErrorResponse()
                {
                    Error = "You dont have the required role"
                });
            }

            if (article.Id == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "No se especifico un articulo a editar"
                });
            }

            var entityExistsFlag = _context.Articles.Any(a => a.Id == article.Id);

            if (!entityExistsFlag)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "Article Invalid"
                });
            }

            if (article.OriginalFlag)
            {
                var original = new OriginalItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleId = article.Id
                };
                article.OriginalSerial = null;
                _context.Add(original);
            }

            if (!article.OriginalFlag)
            {
                var original = _context.Originals.Where(o => o.ArticleId == article.Id);
                _context.Remove(original);
            }

            _context.Update(article);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "El articulo se actualizo con exito"
            });
        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteArticle([FromQuery] string id)
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
                    Error = "No se especifico un articulo para eliminar"
                });
            }

            var dbArticle = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (dbArticle == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Error = "El Articulo que intentas eliminar ya no existe"
                });
            }

            _context.Remove(dbArticle);
            _context.SaveChanges();

            return Ok(new OkResponse()
            {
                Response = "El articulo se elimino con exito"
            });
        }
    }
}