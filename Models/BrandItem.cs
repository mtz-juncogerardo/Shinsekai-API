using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Models
{
    public class BrandItem: ITag
    {
        [MaxLength(36)] public string Id { get; set; }
        [MaxLength(50)] [Required] public string Name { get; set; }
        public List<ArticleItem> Articles { get; set; }
    }
}