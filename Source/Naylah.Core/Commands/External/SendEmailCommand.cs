using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Commands.External
{
    public class SendEmailCommand
    {
        public List<string> To { get; set; }
        public List<string> Bcc { get; set; }
        public List<string> Cc { get; set; }

        public string FromDisplayName { get; set; }
        public string FromAddress { get; set; }

        public string Subject { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }

        public SendEmailCommand()
        {
            To = new List<string>();
            Bcc = new List<string>();
            Cc = new List<string>();
        }

        public SendEmailCommand(
            string to,
            string bcc,
            string cc,

            string fromAddress,
            string fromDisplayName,
            string subject,
            string text,
            string html
            )
        {
            To = new List<string>() { to };
            Bcc = new List<string>() { bcc };
            Cc = new List<string>() { cc };

            FromDisplayName = fromDisplayName;
            FromAddress = fromAddress;
            Subject = subject;
            Text = text;
            Html = html;
        }
    }
}
