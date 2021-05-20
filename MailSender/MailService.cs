using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Shinsekai_API.Config;

namespace Shinsekai_API.MailSender
{
    public abstract class MailService
    {
        private readonly string _receiverEmail;
        private readonly string _subject;
        private readonly ApiConfiguration _configuration;

        protected MailService(string subject, string receiverEmail, IConfiguration configuration)
        {
            _subject = subject;
            _receiverEmail = receiverEmail;
            _configuration = new ApiConfiguration(configuration);
        }

        public void SendEmail()
        {
            var message = new MailMessage(_configuration.MailServiceEmail, _receiverEmail)
            {
                Subject = _subject,
                Body = GetEmailTemplate(),
                IsBodyHtml = true
            };
            var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Credentials = new NetworkCredential(_configuration.MailServiceEmail, _configuration.MailServicePassword),
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
