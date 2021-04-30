using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class FavoriteItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string UserId { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        public ArticleItem Article { get; set; }
        public UserItem User { get; set; }
    }
}