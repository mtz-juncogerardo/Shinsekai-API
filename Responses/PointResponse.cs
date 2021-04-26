using System.Collections.Generic;
using Shinsekai_API.Models;

namespace Shinsekai_API.Responses
{
    public class PointResponse
    {
        public List<PointItem> ExpiredPoints { get; set; }
        public List<PointItem> ValidPoints { get; set; }
        public int TotalValid { get; set; }
        public int TotalExpired { get; set; }
    }
}