using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class ShoppingCartArticleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string UserId { get; set; }
        public UserItem User { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        public ArticleItem Article { get; set; }
    }
}