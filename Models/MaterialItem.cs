using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class MaterialItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(50)] public string Name { get; set; }
        public List<MaterialArticleItem> Materials { get; set; }
    }
}