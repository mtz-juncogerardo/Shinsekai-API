using System;
using System.Collections.Generic;
using System.Linq;
using Shinsekai_API.Interfaces;
using Shinsekai_API.Models;
using Shinsekai_API.Responses;

namespace Shinsekai_API.Requests
{
    public class ArticleRequest : ArticleItem, IArticle
    {
        public List<Tag> Brand { get; set; }
        public List<Image> Images { get; set; }

        public List<Tag> Animes { get; set; }
        public List<Tag> Materials { get; set; }
        public List<Tag> Lines { get; set; }
        public int Quantity { get; set; }

        public void UpdateContext(ShinsekaiApiContext context)
        {
            var currentValues = context.Articles.FirstOrDefault(a => a.Id == Id);
            var existsAsOriginal = context.Originals.Any(o => o.ArticleId == Id);

            if (currentValues != null)
            {
                if (OriginalFlag && !existsAsOriginal)
                {
                    var original = new OriginalItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ArticleId = Id
                    };
                    context.Originals.Add(original);
                    currentValues.OriginalSerial = null;
                    OriginalSerial = null;
                }
                else if (!OriginalFlag && existsAsOriginal)
                {
                    var original = context.Originals.FirstOrDefault(o => o.ArticleId == Id);
                    context.Remove(original);
                }
                currentValues.Details = Details ?? currentValues.Details;
                currentValues.OriginalSerial = OriginalSerial ?? currentValues.OriginalSerial;
                currentValues.Height = Height == 0 ? currentValues.Height : Height;
                currentValues.Name = Name ?? currentValues.Name;
                currentValues.Price = Price == 0 ? currentValues.Price : Price;
                currentValues.Stock = Stock;
                currentValues.BrandId = BrandId ?? currentValues.BrandId;
                currentValues.DateAdded = currentValues.DateAdded;
                currentValues.DiscountPrice = DiscountPrice == 0 ? currentValues.DiscountPrice : DiscountPrice;
                currentValues.OriginalFlag = OriginalFlag != currentValues.OriginalFlag ? OriginalFlag : currentValues.OriginalFlag;
            }

            context.SaveChanges();
        }
    }
}