using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.MailSender
{
    public class RecoverCredentialsMail : MailService
    {
        private const string Subject = "Shinsekai restablece tu contraseña";
        private readonly string _buttonLink;
        public RecoverCredentialsMail(string receiverEmail, string buttonLink, IConfiguration configuration)
        : base(Subject, receiverEmail, configuration)
        {
            _buttonLink = buttonLink;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>To recover your account go to this link</h1><a href=\"{_buttonLink}\" target=__blank>{_buttonLink}</a>";
        }
    }
}
