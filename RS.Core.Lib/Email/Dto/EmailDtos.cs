using System.Collections.Generic;

namespace RS.Core.Lib.Email
{
    public class EmailBasicTemplateDto : EmailDto, IEmailDto
    {
        public string BackgroundColor { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string ButtonValue { get; set; }
        public string URL { get; set; }
    }

    public class EmailDto : IEmailDto
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailDto()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }
    }

    public interface IEmailDto
    {
        List<string> To { get; set; }
        List<string> Cc { get; set; }
        List<string> Bcc { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }

}
