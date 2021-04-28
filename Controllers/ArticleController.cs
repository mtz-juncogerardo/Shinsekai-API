using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public ArticleController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetArticles([FromQuery] string search, bool byAnime, bool byMaterial, bool byLine)
        {
            var dbArticles = new List<ArticleItem>();
            
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
            
            return Ok(new
            {
                Result = dbArticles
            });
        }
    }
}