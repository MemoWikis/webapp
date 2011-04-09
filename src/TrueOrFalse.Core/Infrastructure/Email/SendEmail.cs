using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TrueOrFalse.Core.Infrastructure
{
    public class SendEmail
    {
        public void Run(MailMessage mail)
        {
            var smtpClient = new SmtpClient(Settings.SmtpServer);
            smtpClient.Credentials = new NetworkCredential(Settings.SmtpUser, Settings.SmtpPass);
            smtpClient.Send(mail);
        }
    }
}
