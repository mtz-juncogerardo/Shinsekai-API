using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Shinsekai_API.MailSender
{
    public class EmailValidationMail : MailService
    {
        public string ButtonLink { get; }
        public EmailValidationMail(string reciverEmail, string buttonLink)
        {
            ButtonLink = buttonLink;
            ReciverEmail = reciverEmail;
            Subject = "Welcome to TripPlanner";
        }

        public override string GetEmailTemplate()
        {
            return $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head><meta charset=\"UTF-8\"><meta content=\"width=device-width, initial-scale=1\" name=\"viewport\"><meta name=\"x-apple-disable-message-reformatting\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta content=\"telephone=no\" name=\"format-detection\"><title></title> <!--[if (mso 16)]><style type=\"text/css\">a{{text - decoration:none}}</style><![endif]--> <!--[if gte mso 9]><style>sup{{font - size:100% !important}}</style><![endif]--> <!--[if gte mso 9]> <xml> <o:OfficeDocumentSettings> <o:AllowPNG></o:AllowPNG> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--></head><body><div class=\"es-wrapper-color\"> <!--[if gte mso 9]> <v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\"> <v:fill type=\"tile\" color=\"#eeeeee\"></v:fill> </v:background> <![endif]--><table class=\"es-wrapper\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-email-paddings\" valign=\"top\"><table class=\"es-content esd-footer-popover\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\"><tbody><tr><td class=\"esd-stripe\" align=\"center\"><table class=\"es-content-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tbody><tr><td class=\"esd-structure es-p40t es-p35r es-p35l\" esd-custom-block-id=\"7685\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-container-frame\" width=\"530\" valign=\"top\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-block-text es-m-txt-l es-p15t\" align=\"left\"><h3>Hello,</h3></td></tr><tr><td class=\"esd-block-text es-p15t es-p10b\" align=\"left\"><p style=\"font-size: 16px; color: #777777; font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;\">Welcome to TripPlanner to continue your registration please click the button below.</p></td></tr><tr><td class=\"esd-block-spacer es-p20t es-p15b\" align=\"center\" style=\"font-size:0\"><table width=\"100%\" height=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody><tr><td style=\"border-bottom: 3px solid #eeeeee; background: rgba(0, 0, 0, 0) none repeat scroll 0% 0%; height: 1px; width: 100%; margin: 0px;\"></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr><td class=\"esd-structure es-p30t es-p35b es-p35r es-p35l\" esd-custom-block-id=\"7685\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-container-frame\" width=\"530\" valign=\"top\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tbody><tr><td class=\"esd-block-text\" align=\"center\"><h2 style=\"color: #333333;\">NEXT STEP</h2></td></tr><tr><td class=\"esd-block-text es-p15t\" align=\"center\"><p style=\"font-size: 16px; color: #777777;\">Please confirm your</p><p style=\"font-size: 16px; color: #777777;\">email address to get started.</p></td></tr><tr><td class=\"esd-block-button es-p30t es-p15b\" align=\"center\"><span class=\"es-button-border\" style=\"background: #ed8e20 none repeat scroll 0% 0%;\"><a href=\"{ButtonLink}\" class=\"es-button\" target=\"_blank\" style=\"font-weight: normal; border-width: 15px 30px; background: #ed8e20 none repeat scroll 0% 0%; border-color: #ed8e20; color: #ffffff; font-size: 18px;\">Confirm account</a></span></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></div></body></html>";
        }
    }
}
