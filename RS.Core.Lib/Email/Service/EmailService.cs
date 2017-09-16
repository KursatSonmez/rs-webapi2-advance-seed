using RS.Core.Const;
using System;
using System.IO;
using System.Net.Mail;

namespace RS.Core.Lib.Email
{
    public interface IEmailService
    {
        void SendMail(EmailDto model, bool isBodyHtml = true);
        void SendMailBasicTemplate(EmailBasicTemplateDto model, string templatePath = null);
    }

    public class EmailService : IEmailService
    {
        private void CheckAnyRecipientExists(IEmailDto email)
        {
            if ((email.To == null || email.To.Count == 0) && (email.Cc == null || email.Cc.Count == 0) && (email.Bcc == null || email.Bcc.Count == 0))
                throw new Exception(Messages.EMW0001);
        }

        public void SendMail(EmailDto model, bool isBodyHtml = true)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();

            CheckAnyRecipientExists(model);

            if (model.To != null && model.To.Count > 0)
                foreach (var to in model.To)
                    mailMessage.To.Add(new MailAddress(to));

            if (model.Cc != null && model.Cc.Count > 0)
                foreach (var cc in model.Cc)
                    mailMessage.CC.Add(new MailAddress(cc));

            if (model.Bcc != null && model.Bcc.Count > 0)
                foreach (var bcc in model.Bcc)
                    mailMessage.Bcc.Add(new MailAddress(bcc));

            mailMessage.Subject = model.Subject;
            mailMessage.Body = model.Body;
            mailMessage.IsBodyHtml = isBodyHtml;

            smtpClient.Send(mailMessage);

        }

        public void SendMailBasicTemplate(EmailBasicTemplateDto model, string templatePath = null)
        {
            // Checking recipient before load template
            CheckAnyRecipientExists(model);

            string template;
            if (templatePath != null)
                template = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + templatePath);
            else
                template = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"..\RS.Core.Api\Content\Email\BasicTemplate.html");

            template = template.Replace("**BackgroundColor**", model.BackgroundColor);
            template = template.Replace("**Header**", model.Header);
            template = template.Replace("**Content**", model.Content);
            template = template.Replace("**URL**", model.URL);
            template = template.Replace("**ButtonValue**", model.ButtonValue);

            EmailDto emailDto = new EmailDto
            {
                To = model.To,
                Cc = model.Cc,
                Bcc = model.Bcc,
                Subject = model.Subject,
                Body = template
            };

            SendMail(emailDto, true);
        }
    }
}
