using Microsoft.EntityFrameworkCore;

namespace Shinsekai_API.Models
{
    public class ShinsekaiApiContext : DbContext
    {
        public ShinsekaiApiContext(DbContextOptions<ShinsekaiApiContext> options) : base(options)
        {
        }

        public DbSet<AnimeArticleItem> AnimesArticles { get; set; }
        public DbSet<AnimeItem> Animes { get; set; }
        public DbSet<ArticleItem> Articles { get; set; }
        public DbSet<AuthParamItem> AuthParams { get; set; }
        public DbSet<BrandItem> Brands { get; set; }
        public DbSet<CarouselItem> Carousels { get; set; }
        public DbSet<DeliveryItem> Deliveries { get; set; }
        public DbSet<FavoriteItem> Favorites { get; set; }
        public DbSet<ImageItem> Images { get; set; }
        public DbSet<LineArticleItem> LinesArticles { get; set; }
        public DbSet<LineItem> Lines { get; set; }
        public DbSet<LocationItem> Locations { get; set; }
        public DbSet<MaterialItem> Materials { get; set; }
        public DbSet<MaterialArticleItem> MaterialsArticles { get; set; }
        public DbSet<OriginalItem> Originals { get; set; }
        public DbSet<ReplicaItem> Replicas { get; set; }
        public DbSet<PointItem> Points { get; set; }
        public DbSet<PromotionItem> Promotions { get; set; }
        public DbSet<PurchaseItem> Purchases { get; set; }
        public DbSet<PurchaseArticleItem> PurchasesArticles { get; set; }
        public DbSet<QuestionItem> Questions { get; set; }
        public DbSet<RequestItem> Requests { get; set; }
        public DbSet<SaleItem> Sales { get; set; }
        public DbSet<ShoppingCartArticleItem> ShoppingCartArticles { get; set; }
        public DbSet<UserItem> Users { get; set; }
    }
}