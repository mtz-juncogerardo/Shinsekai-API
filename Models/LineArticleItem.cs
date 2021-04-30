using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class LineArticleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        [Required] [MaxLength(36)] public string LineId { get; set; }
        public LineItem Line { get; set; }
        public ArticleItem Article { get; set; }
    }
}