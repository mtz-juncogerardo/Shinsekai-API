using System;
using System.Net;
using System.Net.Mail;
using Shinsekai_API.Config;

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
            var message = new MailMessage(ApiConfiguration.MailServiceEmail, _receiverEmail)
            {
                Subject = _subject,
                Body = GetEmailTemplate(),
                IsBodyHtml = true
            };
            var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Credentials = new NetworkCredential(ApiConfiguration.MailServiceEmail, ApiConfiguration.MailServicePassword),
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
