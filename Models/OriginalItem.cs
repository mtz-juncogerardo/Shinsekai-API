using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class OriginalItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [MaxLength(36)] [Required] public string ArticleId { get; set; }
        public ArticleItem Article { get; set; }
    }
}