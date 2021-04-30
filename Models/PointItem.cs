using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PointItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [DefaultValue("10-09-92")] public DateTime ExpirationDate { get; set; }
        [Required] [DefaultValue(7.2)] public decimal Amount { get; set; }
        [Required] [MaxLength(36)] public string UserId { get; set; }
        public UserItem User { get; set; }
    }
}