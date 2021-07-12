using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PromotionItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(150)] public string ImagePath { get; set; }
        [MaxLength(100)] public string RedirectPath { get; set; }
        [DefaultValue(false)]public bool AppearsOnLeft { get; set; }
        [DefaultValue(false)]public bool AppearsOnRight { get; set; }
    }
}