using System.Collections.Generic;
using Shinsekai_API.Models;

namespace Shinsekai_API.Responses
{
    public class PointResponse
    {
        public List<PointItem> Points { get; set; }
        public decimal TotalValid { get; set; }
        public decimal TotalExpired { get; set; }
    }
}