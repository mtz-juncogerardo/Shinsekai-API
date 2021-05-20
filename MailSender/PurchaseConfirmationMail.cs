using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.MailSender
{
    public class PurchaseConfirmationMail : MailService
    {
        private readonly string _purchaseId;
        private const string Subject = "Shinsekai - Thank you for your purchase";

        public PurchaseConfirmationMail(string receiverEmail, string purchaseId, IConfiguration configuration) 
            : base(Subject, receiverEmail, configuration)
        {
            _purchaseId = purchaseId;
        }

        protected override string GetEmailTemplate()
        {
            return $"Este es el folio de tu compra: {_purchaseId}, no lo pierdas pues con este podras hacer alguna reclamación";
        }
    }
}