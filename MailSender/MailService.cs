using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Shinsekai_API.MailSender
{
    public abstract class MailService
    {
        public string ReciverEmail { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }

        public void SendEmail()
        {
            MailMessage message = new MailMessage("gerardo@mtzjunco.com", ReciverEmail);
            message.Subject = Subject;
            message.Body = GetEmailTemplate();
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("mail.privateemail.com");

            client.Credentials = new NetworkCredential("gerardo@mtzjunco.com", "C0dyng99");
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}",
                            ex.ToString());
            }
        }

        public abstract string GetEmailTemplate();
    }
}
