using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Requests
{
    public class ContactRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string PurchaseId { get; set; }
        [Required] public string Message { get; set; }
        [Required] public string Name { get; set; }
    }
}