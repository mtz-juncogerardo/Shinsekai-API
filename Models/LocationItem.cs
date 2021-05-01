using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Shinsekai_API.Models
{
    public class LocationItem
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(36)] public string Id { get; set; }
        public DeliveryItem Delivery { get; set; }
    }
}