using System.Collections;

namespace RS.Core.Lib.Email
{
    public class EmailDto
    {
        public ArrayList To { get; set; }
        public ArrayList Cc { get; set; }
        public ArrayList Bcc { get; set; }
        public string Subject { get; set; }
        public string BackgroundColor { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string ButtonValue { get; set; }
        public string URL { get; set; }
    }
}
