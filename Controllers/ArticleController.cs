using System;
using System.Collections.Generic;
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
        public IActionResult GetArticles([FromQuery] string page, string search, bool byName, bool byAnime, bool byMaterial, bool byLine)
        {
            var pageNum = page == null ? 1 : int.Parse(page);
            var elementsToSkip = (pageNum - 1) * _numberOfElementsInPage;
            var dbArticles = _context.Articles.ToList();

            if (byAnime)
            {
                dbArticles = _context.AnimesArticles.Join(_context.Animes,
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
            if (byMaterial)
            {
                dbArticles = _context.MaterialsArticles.Join(_context.Materials,
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
            if (byLine)
            {
                dbArticles = _context.LinesArticles.Join(_context.Lines,
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

            var responseArticles = dbArticles.Skip(elementsToSkip).Take(_numberOfElementsInPage);
            var count = dbArticles.Count();
            var maxPages = (int)Math.Ceiling(count / (decimal)_numberOfElementsInPage);

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
        public IActionResult SaveArticle([FromBody] ArticleItem article)
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

            article.Id = Guid.NewGuid().ToString();

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
                    Error = "No se especifico un articulo a editar"
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