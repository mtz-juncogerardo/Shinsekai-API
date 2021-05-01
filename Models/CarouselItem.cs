using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class CarouselItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [MaxLength(100)] [Required] public string ImagePath { get; set; }
        [MaxLength(100)] public string RedirectPath { get; set; }
    }
}