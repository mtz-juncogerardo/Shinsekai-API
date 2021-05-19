using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Models
{
    public class AnimeItem: ITag
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(50)] public string Name { get; set; }
        public List<AnimeArticleItem> AnimesArticles { get; set; } 
    }
}