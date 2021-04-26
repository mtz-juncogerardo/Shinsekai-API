using System;
using System.Collections.Generic;
using Shinsekai_API.Models;

namespace Shinsekai_API.Responses
{
    public class PurchaseResponse
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public List<ArticleItem> Articles { get; set; }
    }
}