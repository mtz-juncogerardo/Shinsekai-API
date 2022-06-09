using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.MailSender
{
    public class PurchaseConfirmationMail : MailService
    {
        private readonly string _purchaseId;
        private const string Subject = "Shinsekai - Thank you for your purchase";

        public PurchaseConfirmationMail(string receiverEmail, string purchaseId) 
            : base(Subject, receiverEmail)
        {
            _purchaseId = purchaseId;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>Gracias por comprar en Shinsekai Shop</h1> <p>Este es el folio de tu compra: <strong>{_purchaseId}</strong>, no lo pierdas pues con este podras hacer alguna reclamación</p>";
        }
    }
}