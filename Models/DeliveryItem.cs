using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Models
{
    public class DeliveryItem
    {
        [Required] [MaxLength(36)] public string Id { get; set; }
        [Required] [MaxLength(50)] public string Parcel { get; set; }
        [Required] [DefaultValue("10-09-92")] public DateTime EstimatedDays { get; set; }
        [Required] [MaxLength(36)] public string LocationId { get; set; }
        public LocationItem Location { get; set; }
    }
}