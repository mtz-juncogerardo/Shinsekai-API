using System;
using System.Collections.Generic;
using System.Linq;
using Shinsekai_API.Interfaces;
using Shinsekai_API.Models;

namespace Shinsekai_API.Responses
{
    public class ArticleResponse : IArticle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Height { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }
        public bool OriginalFlag { get; set; }
        public string Details { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Tag> Brand { get; set; }
        public List<Image> Images { get; set; }

        public List<Tag> Animes { get; set; }
        public List<Tag> Materials { get; set; }
        public List<Tag> Lines { get; set; }
        public int TimesSold { get; set; }
        public string OriginalSerial { get; set; }

        public ArticleResponse(ArticleItem article, ShinsekaiApiContext context)
        {
            Id = article.Id;
            Animes = GetAnimes(context, article.Id).ToList();
            Brand = GetBrands(context, article.BrandId).ToList();
            Materials = GetMaterials(context, article.Id).ToList();
            Lines = GetLines(context, article.Id).ToList();
            Images = GetImages(context, article.Id).ToList();
            TimesSold = context.Sales.Count(s => s.ArticleId == article.Id);
            Name = article.Name;
            Details = article.Details;
            Height = article.Height;
            OriginalSerial = context.Originals.FirstOrDefault(o => o.ArticleId == article.Id)?.Id;
            Price = article.Price;
            Stock = article.Stock;
            DateAdded = article.DateAdded;
            DiscountPrice = article.DiscountPrice;
            OriginalFlag = article.OriginalFlag;
        }

        private static IEnumerable<Tag> GetAnimes(ShinsekaiApiContext context, string articleId)
        {
            return context.AnimesArticles.Join(context.Animes,
                    aa => aa.AnimeId,
                    a => a.Id,
                    (aa, a) => new
                    {
                        AnimeArticle = aa,
                        Anime = a
                    }).Where(a => a.AnimeArticle.ArticleId == articleId)
                .Select(a => new Tag(a.Anime));
        }

        private static IEnumerable<Tag> GetMaterials(ShinsekaiApiContext context, string articleId)
        {
            return context.MaterialsArticles.Join(context.Materials,
                    m => m.MaterialId,
                    a => a.Id,
                    (m, a) => new
                    {
                        MaterialArticle = m,
                        Material = a
                    }).Where(m => m.MaterialArticle.ArticleId == articleId)
                .Select(m => new Tag(m.Material));
        }

        private static IEnumerable<Tag> GetLines(ShinsekaiApiContext context, string articleId)
        {
            return context.LinesArticles.Join(context.Lines,
                    la => la.LineId,
                    l => l.Id,
                    (la, l) => new
                    {
                        LineArticle = la,
                        Line = l
                    }).Where(l => l.LineArticle.ArticleId == articleId)
                .Select(l => new Tag(l.Line));
        }

        private static IEnumerable<Image> GetImages(ShinsekaiApiContext context, string articleId)
        {
            return context.Images.Where(i => i.ArticleId == articleId).Select(i => new Image(i));
        }
        
        private static IEnumerable<Tag> GetBrands(ShinsekaiApiContext context, string brandId)
        {
            return context.Brands.Where(b => b.Id == brandId).Select(b => new Tag(b));
        }
    }
}