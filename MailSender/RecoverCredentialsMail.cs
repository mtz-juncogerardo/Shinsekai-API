using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shinsekai_API.MailSender
{
    public class RecoverCredentialsMail : MailService
    {
        public string ButtonLink { get; set; }
        public RecoverCredentialsMail(string reciverEmail, string buttonLink)
        {
            ReciverEmail = reciverEmail;
            ButtonLink = buttonLink;
            Subject = "You requested your password";
        }

        public override string GetEmailTemplate()
        {
            return $"<h1>To recover your account go to this link</h1><a href=\"{ButtonLink}\" target=__blank>{ButtonLink}</a>";
        }
    }
}
