using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class AuthParamItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(100)] public string Salt { get; set; }
        [Required] [MaxLength(300)] public string Password { get; set; }
        public UserItem User { get; set; }
    }
}