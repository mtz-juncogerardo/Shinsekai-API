using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class QuestionItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(200)] public string Question { get; set; }
        [Required] [MaxLength(300)] public string Answer { get; set; }
    }
}