using System;
using System.Collections.Generic;
using System.ComponentModel;
using Shinsekai_API.Models;

namespace Shinsekai_API.Interfaces
{
    public interface IArticle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Height { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }
        public bool OriginalFlag { get; set; }
        public string Details { get; set; }
        public DateTime DateAdded { get; set; }
        public int Quantity { get; set; }
    }
}