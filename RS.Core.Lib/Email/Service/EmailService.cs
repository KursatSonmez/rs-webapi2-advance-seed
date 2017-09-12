using RS.Core.Const;
using System;
using System.IO;
using System.Net.Mail;

namespace RS.Core.Lib.Email
{
    public interface IEmailService
    {
        void SendMail(EmailDto model, string tempLocation = null);
    }
    public class EmailService : IEmailService
    {
        public void SendMail(EmailDto model, string tempLocation = null)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();

            string temp;
            if (tempLocation != null)
                temp = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + tempLocation);
            else
                temp = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"..\RS.Core.Api\Content\Email\EmailTemplate.html");

            if ((model.To == null || model.To.Count == 0) && (model.Cc == null || model.Cc.Count == 0) && (model.Bcc == null || model.Bcc.Count == 0))
                throw new Exception(Messages.EMW0001);

            if (model.To != null && model.To.Count > 0)
                foreach (var to in model.To)
                {
                    mailMessage.To.Add(new MailAddress(to.ToString()));
                }          

            if (model.Cc != null && model.Cc.Count > 0)
                foreach (var cc in model.Cc)
                {
                    mailMessage.CC.Add(new MailAddress(cc.ToString()));
                }

            if (model.Bcc != null && model.Bcc.Count > 0)
                foreach (var bcc in model.Bcc)
                {
                    mailMessage.Bcc.Add(new MailAddress(bcc.ToString()));
                }

            temp = temp.Replace("**BackgroundColor**", model.BackgroundColor);
            temp = temp.Replace("**Header**", model.Header);
            temp = temp.Replace("**Content**", model.Content);
            temp = temp.Replace("**URL**", model.URL);
            temp = temp.Replace("**ButtonValue**", model.ButtonValue);

            mailMessage.Subject = model.Subject;
            mailMessage.Body = temp;
            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);

        }
    }
}
