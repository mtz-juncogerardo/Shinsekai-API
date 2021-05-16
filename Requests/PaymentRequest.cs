using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shinsekai_API.Models;

namespace Shinsekai_API.Requests
{
    public class PaymentRequest
    {
        [Required] public List<ArticleRequest> Articles { get; set; }
        [Required] public string SuccessUrl { get; set; }
        [Required] public string ErrorUrl { get; set; }
        public bool PayWithPoints { get; set; }
    }
}