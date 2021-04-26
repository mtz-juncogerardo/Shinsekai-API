using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;

namespace Shinsekai_API.Controllers
{
    [ApiController]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly ShinsekaiApiContext _context;

        public ArticleController(ShinsekaiApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetArticles([FromBody] ArticleRequest req)
        {
            var dbArticles = new List<ArticleItem>();
            if (req.ByAnime)
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
            if (req.ByMaterial)
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
            if (req.ByLine)
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
            
            return Ok(new
            {
                Result = dbArticles
            });
        }
    }
}