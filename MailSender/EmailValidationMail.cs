using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.MailSender
{
    public class EmailValidationMail : MailService
    {
        private const string Subject = "Welcome to Shinsekai Shop";

        private readonly string _buttonLink;
        public EmailValidationMail(string receiverEmail, string buttonLink, IConfiguration configuration)
        : base(Subject, receiverEmail, configuration)
        {
            _buttonLink = buttonLink;
        }

        protected override string GetEmailTemplate()
        {
            return $"<h1>Gracias por registrarte en Shinsekai Shop</h1> <p>Para terminar tu registro por favor haz click en el siguiente enlace.</p> <a href=\"{_buttonLink}\" target=_blank>Click Aquí</a> <br>" +
                   $"<p>Si no puedes acceder al link, copia y pega el siguiente enlace en tu navegador:<p><br>{_buttonLink}";
        }
    }
}