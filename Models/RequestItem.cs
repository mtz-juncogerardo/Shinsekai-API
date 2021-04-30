using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class RequestItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(36)] public string PurchaseId { get; set; }
        public PurchaseItem Purchase { get; set; }
        [Required] [MaxLength(300)] public string Details { get; set; }
        [Required] [MaxLength(50)] public string Email { get; set; }
    }
}