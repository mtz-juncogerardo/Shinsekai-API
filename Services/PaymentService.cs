using System;
using System.Collections.Generic;
using System.Linq;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Stripe.Checkout;

namespace Shinsekai_API.Services
{
    public class PaymentService : PaymentRequest
    {
        private const string Currency = "mxn";
        private int _cashPoints;
        public List<SessionLineItemOptions> LineItems { get; }

        public PaymentService(PaymentRequest payment)
        {
            Articles = payment.Articles;
            SuccessUrl = payment.SuccessUrl;
            ErrorUrl = payment.ErrorUrl;
        }

        public PaymentService(PaymentRequest payment, int cashPoints)
            : this(payment)
        {
            _cashPoints = cashPoints;
            UpdateArticlesWithDiscount();
        }

        public List<SessionLineItemOptions> CreateLineItems()
        {
            return Articles.Select(article => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = Convert.ToInt64(article.Price - article.DiscountPrice),
                    Currency = Currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = article.Name,
                    },
                },
                Quantity = article.Quantity
            }).ToList();
        }

        private void UpdateArticlesWithDiscount()
        {
            var totalPrice = Articles.Sum(a => (a.Price - a.DiscountPrice) * a.Quantity);
            var totalQuantity = Articles.Sum(a => a.Quantity);
            var totalWithDiscount = totalPrice - _cashPoints;
            _cashPoints = totalWithDiscount < 0 ? (int)Math.Abs(totalWithDiscount) : 0;

            foreach (var article in Articles)
            {
                article.Price = _cashPoints == 0 ? totalWithDiscount / totalQuantity : decimal.Zero;
            }
        }
    }
}