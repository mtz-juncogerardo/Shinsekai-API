using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace Shinsekai_API.Models
{
    public class ArticleItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [MaxLength(50)] [Required] public string Name { get; set; }
        [DefaultValue(7.2)] public decimal Height { get; set; }
        [Required] [DefaultValue(7.2)] public decimal Price { get; set; }
        [DefaultValue(7.2)] public decimal DiscountPrice { get; set; }
        [DefaultValue(0)] public int Stock { get; set; }
        [Required] public bool OriginalFlag { get; set; }
        [MaxLength(36)] public string OriginalSerial { get; set; }
        [MaxLength(200)] public string Details { get; set; }
        [Required] [DefaultValue("10-09-92")] public DateTime DateAdded { get; set; }
        [Required] public string BrandId { get; set; }
        public OriginalItem Original { get; set; }
        public BrandItem Brand { get; set; }
        public List<SaleItem> Sales { get; set; }
        public List<ImageItem> Images { get; set; }
        public List<AnimeItem> Animes { get; set; }
        public List<FavoriteItem> Favorites { get; set; }
        public List<PurchaseArticleItem> PurchasesArticles { get; set; }
        public List<MaterialArticleItem> MaterialsArticles { get; set; }
        public List<ShoppingCartArticleItem> ShoppingCartsArticles { get; set; }
    }
}