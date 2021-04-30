using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class MaterialArticleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        [Required] [MaxLength(36)] public string MaterialId { get; set; }
        public MaterialItem Material { get; set; }
        public ArticleItem Article { get; set; }
    }
}