using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TrueOrFalse.Core.Infrastructure
{
    public class SendEmail : IRegisterAsInstancePerLifetime
    {
        public void Run(MailMessage mail)
        {
            var smtpClient = new SmtpClient(WebConfigSettings.SmtpServer);
            smtpClient.Credentials = new NetworkCredential(WebConfigSettings.SmtpUser, WebConfigSettings.SmtpPass);
            smtpClient.Send(mail);
        }
    }
}
