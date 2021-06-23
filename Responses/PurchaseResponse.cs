using System;
using System.Collections.Generic;
using Shinsekai_API.Models;

namespace Shinsekai_API.Responses
{
    public class PurchaseResponse 
    {
        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public IEnumerable<ArticleResponse> Articles { get; set; }
        public decimal Total { get; set; }
        public UserItem Buyer { get; set; }
    }
}