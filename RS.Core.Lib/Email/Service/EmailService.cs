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
                temp = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"..\RS.Core.Lib\Email\Template\EmailTemplate.html");
            for (int i = 0; i < model.EmailGroup.Count; i++)
            {
                mailMessage.To.Add(new MailAddress(model.EmailGroup[i].ToString()));
                mailMessage.Subject = model.Subject;
                temp = temp.Replace("**BackgroundColor**", model.BackgroundColor);
                temp = temp.Replace("**Header**", model.Header);
                temp = temp.Replace("**Content**", model.Content);
                temp = temp.Replace("**URL**", model.URL);
                temp = temp.Replace("**ButtonValue**", model.ButtonValue);
                mailMessage.Body = temp;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
