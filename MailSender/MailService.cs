using System;
using System.Net;
using System.Net.Mail;

namespace Shinsekai_API.MailSender
{
    public abstract class MailService
    {
        private readonly string _receiverEmail;
        private readonly string _subject;

        protected MailService(string subject, string receiverEmail)
        {
            _subject = subject;
            _receiverEmail = receiverEmail;
        }

        public void SendEmail()
        {
            var message = new MailMessage("gerardo@mtzjunco.com", _receiverEmail)
            {
                Subject = _subject,
                Body = GetEmailTemplate(),
                IsBodyHtml = true
            };
            var client = new SmtpClient("mail.privateemail.com")
            {
                Credentials = new NetworkCredential("gerardo@mtzjunco.com", "1#4%645fEkeld98&&dn9(d"),
                Port = 587,
                EnableSsl = true
            };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}",
                            ex);
            }
        }

        protected abstract string GetEmailTemplate();
    }
}
