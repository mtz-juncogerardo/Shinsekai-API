using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PurchaseArticleItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        [MaxLength(36)] public string PurchaseId { get; set; }
        public int Quantity { get; set; }
        public ArticleItem Article { get; set; }
        public PurchaseItem Purchase { get; set; }
    }
}