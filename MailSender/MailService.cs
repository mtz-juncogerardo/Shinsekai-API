using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Shinsekai_API.Config;

namespace Shinsekai_API.MailSender
{
    public abstract class MailService
    {
        private readonly string _receiverEmail;
        private readonly string _subject;
        private readonly ApiConfiguration _configuration;

        protected MailService(string subject, string receiverEmail)
        {
            _subject = subject;
            _receiverEmail = receiverEmail;
            _configuration = new ApiConfiguration();
        }

        public async Task SendEmail()
        {
            var client = new SendGridClient(_configuration.SendGridApiKey);
            var from = new EmailAddress(_configuration.MailServiceEmail, "Shinsekai Team");
            var subject = _subject;
            var to = new EmailAddress(_receiverEmail);
            var htmlContent = GetEmailTemplate();
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        protected abstract string GetEmailTemplate();
    }
}