using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TrueOrFalse.Infrastructure
{
    public class SendEmail : IRegisterAsInstancePerLifetime
    {
        public void Run(MailMessage mail)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Send(mail);
        }
    }
}
