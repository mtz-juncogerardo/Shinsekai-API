using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class UserItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [MaxLength(36)] public string Name { get; set; }
        [Required] [MaxLength(50)] public string Email { get; set; }
        [DefaultValue(false)] public bool Admin { get; set; }
        [MaxLength(12)] [DefaultValue(null)] public string Phone { get; set; }
        [MaxLength(150)] [DefaultValue(null)] public string Address { get; set; }
        [MaxLength(50)] [DefaultValue(null)] public string City { get; set; }
        [MaxLength(36)] public string AuthParamsId { get; set; }
        public AuthParamItem AuthParams { get; set; }
        public List<ShoppingCartArticleItem> SoppingCartArticles { get; set; }
        public List<FavoriteItem> Favorites { get; set; }
        public List<PointItem> Points { get; set; }
        public List<PurchaseItem> Purchases { get; set; }
    }
}