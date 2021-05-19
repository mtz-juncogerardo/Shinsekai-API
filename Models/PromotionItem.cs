using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PromotionItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(100)] public string ImagePath { get; set; }
        [MaxLength(100)] public string RedirectPath { get; set; }
    }
}