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
            return $"<h1>Has solicitado un cambio de contraseña en Shinsekai Shop</h1> <p>Para poder cambiar tu contraseña por favor visita el siguiente link:</p> <a href=\"{_buttonLink}\" target=_blank>Click Aqui</a> <br>" +
                   $"<p>Si no puedes acceder al link, copia y pega el siguiente enlace en tu navegador:<p><br>{_buttonLink}";
        }
    }
}
