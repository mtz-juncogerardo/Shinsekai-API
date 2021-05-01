using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class DeliveryItem
    {
        [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(50)] public string Parcel { get; set; }
        [Required] [DefaultValue(1)] public int EstimatedDays{ get; set; }
        [Required] [MaxLength(36)] public string LocationId { get; set; }
        public LocationItem Location { get; set; }
    }
}