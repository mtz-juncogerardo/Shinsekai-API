using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class AnimeArticleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        [Required] [MaxLength(36)] public string AnimeId { get; set; }
        public AnimeItem Anime { get; set; }
        public ArticleItem Article { get; set; }
    }
}