using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Shinsekai_API.Interfaces;
using Shinsekai_API.Responses;


namespace Shinsekai_API.Models
{
    public class ArticleItem : ITag
    {
        [MaxLength(36)] public string Id { get; set; }
        [MaxLength(50)] public string Name { get; set; }
        public decimal Height { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        [DefaultValue(null)] public int Stock { get; set; }
        [DefaultValue(false)] public bool OriginalFlag { get; set; }
        [MaxLength(200)] public string Details { get; set; }
        public string OriginalSerial { get; set; }
        [DefaultValue("10-09-92")] public DateTime DateAdded { get; set; }
        public string BrandId { get; set; }
        public OriginalItem Original { get; set; }
        public BrandItem Brand { get; set; }
        public List<SaleItem> Sales { get; set; }
        public List<ImageItem> Images { get; set; }
        public List<FavoriteItem> Favorites { get; set; }
        public List<PurchaseArticleItem> PurchasesArticles { get; set; }
        public List<MaterialArticleItem> MaterialsArticles { get; set; }
        public List<AnimeArticleItem> AnimesArticles { get; set; }
        public List<LineArticleItem> LinesArticles { get; set; }
        public List<ShoppingCartArticleItem> ShoppingCartsArticles { get; set; }
    }
}