using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class SaleItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [DefaultValue("10-09-92")] public DateTime SoldDate { get; set; }
        [Required] [MaxLength(36)] public string ArticleId { get; set; }
        public ArticleItem Article { get; set; }
    }
}