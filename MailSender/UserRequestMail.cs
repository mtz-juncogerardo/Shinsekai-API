using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.MailSender
{
    public class UserRequestMail : MailService
    {
        private const string Subject = "Shinsekai - Un usuario mando una solicitud";
        private readonly string _userName;
        private readonly string _purchaseId;
        private readonly string _details;
        private readonly string _userEmail;

        public UserRequestMail(string receiverEmail, string userName, string purchaseId, string details, string userEmail) 
            : base(Subject, receiverEmail)
        {
            _userName = userName;
            _userEmail = userEmail;
            _purchaseId = purchaseId;
            _details = details;
        }
        protected override string GetEmailTemplate()
        {
            return $"<p>El usuario: <strong>{_userName}</strong><br> registrado con el mail: <strong>{_userEmail}</strong><br> con el Id de compra: <strong>{_purchaseId}</strong> tiene el siguiente comentario: </p> <h4>{_details}</h4>";
        }
    }
}