using System.ComponentModel.DataAnnotations;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Models
{
    public class ImageItem: IImage
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(150)] public string Path { get; set; }
        [MaxLength(36)] public string ArticleId { get; set; }
        public long Order { get; set; }
        public ArticleItem Article { get; set; }
    }
}