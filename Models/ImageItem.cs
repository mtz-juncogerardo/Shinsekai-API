using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class ImageItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(100)] public string Path { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        public ArticleItem Article { get; set; }
    }
}