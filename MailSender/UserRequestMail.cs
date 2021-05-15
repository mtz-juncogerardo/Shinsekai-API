namespace Shinsekai_API.MailSender
{
    public class UserRequestMail : MailService
    {
        private const string Subject = "Shinsekai - Un usuario mando una solicitud";
        private readonly string _userName;
        private readonly string _purchaseId;
        private readonly string _details;

        public UserRequestMail(string receiverEmail, string userName, string purchaseId, string details) 
            : base(Subject, receiverEmail)
        {
            _userName = userName;
            _purchaseId = purchaseId;
            _details = details;
        }
        protected override string GetEmailTemplate()
        {
            return $"El usuario {_userName} con el Id de compra {_purchaseId} tiene el siguiente comentario: {_details}";
        }
    }
}