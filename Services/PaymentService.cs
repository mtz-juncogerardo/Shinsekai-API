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
        public List<SessionLineItemOptions> LineItems { get; }

        public PaymentService(PaymentRequest payment)
        {
            Articles = payment.Articles;
            LineItems = CreateLineItems();
            SuccessUrl = payment.SuccessUrl;
            ErrorUrl = payment.ErrorUrl;
        }

        public List<SessionLineItemOptions> CreateLineItems()
        {
            return Articles.Select(article => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = Convert.ToInt64(article.Price),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = article.Name,
                        },
                    },
                    Quantity = article.Quantity
                })
                .ToList();
        }
    }
}