using System.ComponentModel.DataAnnotations;

namespace Shinsekai_API.Requests
{
    public class DeliveryRequest
    {
        public string Id { get; set; }
        public string Parcel { get; set; }
        public int EstimatedDays{ get; set; }
        public string LocationId { get; set; }
    }
}