using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PurchaseArticleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        [Required] [MaxLength(36)] public string PurchaseId { get; set; }
        public ArticleItem Article { get; set; }
        public PurchaseItem Purchase { get; set; }
    }
}