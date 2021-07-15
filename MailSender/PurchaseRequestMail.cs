using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Interfaces;
using Shinsekai_API.Models;

namespace Shinsekai_API.MailSender
{
    public class PurchaseRequestMail: MailService
    {
        private readonly PurchaseItem _purchaseItem;
        private const string Subject = "Tienes una nueva Compra!";
        private const string mail = "ruben.odisey@gmail.com";

        public PurchaseRequestMail(PurchaseItem purchase, IConfiguration configuration) 
            : base(Subject, mail, configuration)
        {
            _purchaseItem = purchase;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>Nueva Compra!</h1> <p>Este es el id de la compra: <strong>{_purchaseItem.Id}</strong>Puedes consultar los detalles en el panel de administración</p>";
        }
    }
}