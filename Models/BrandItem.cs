using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class BrandItem
    {
        [MaxLength(36)] [Required] public string Id { get; set; }
        [MaxLength(50)] [Required] public string Name { get; set; }
        public List<ArticleItem> Articles { get; set; }
    }
}