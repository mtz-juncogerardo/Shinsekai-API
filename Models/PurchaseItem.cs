using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class PurchaseItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [MaxLength(36)] public string UserId { get; set; }
        public UserItem User { get; set; }
        public string BuyerEmail { get; set; }
        [Required] [DefaultValue("10-09-92")] public DateTime Date { get; set; }
        public List<PurchaseArticleItem> PurchasesArticles { get; set; }
        public List<RequestItem> Requests { get; set; }
    }
}