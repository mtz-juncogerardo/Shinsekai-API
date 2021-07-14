using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shinsekai_API.Models;
using Shinsekai_API.Requests;
using Stripe.Checkout;

namespace Shinsekai_API.Services
{
    public class PaymentService : PaymentRequest
    {
        private const string Currency = "mxn";
        private decimal _cashPoints;
        private bool _disccountApplied;

        public PaymentService(PaymentRequest payment)
        {
            Articles = payment.Articles;
            SuccessUrl = payment.SuccessUrl;
            ErrorUrl = payment.ErrorUrl;
            _cashPoints = 0;
        }

        public PaymentService(PaymentRequest payment, decimal cashPoints)
            : this(payment)
        {
            _cashPoints = cashPoints;
            UpdateArticlesWithDiscount();
        }

        public decimal GetCashPoints()
        {
            return _cashPoints;
        }

        public decimal GetTotal()
        {
            return Articles.Sum(a => (a.Price - a.DiscountPrice) * a.Quantity);
        }

        public List<SessionLineItemOptions> CreateLineItems()
        {
            return Articles.Select(article => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = (_disccountApplied ? decimal.Round(article.Price, 2) : article.Price - article.DiscountPrice) * 100,
                    Currency = Currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = article.Name
                    }
                },
                Quantity = article.Quantity
            }).ToList();
        }

        private void UpdateArticlesWithDiscount()
        {
            var cashPoints = _cashPoints;
            var totalPrice = Articles.Sum(a => (a.Price - a.DiscountPrice) * a.Quantity);
            var totalQuantity = Articles.Sum(a => a.Quantity);
            var totalWithDiscount = totalPrice - cashPoints;
            cashPoints = totalWithDiscount < 0 ? totalWithDiscount : 0;
            _cashPoints -= cashPoints;

            foreach (var article in Articles)
            {
                article.Price = cashPoints == 0 ? totalWithDiscount / totalQuantity : decimal.Zero;
            }

            _disccountApplied = true;
        }
    }
}