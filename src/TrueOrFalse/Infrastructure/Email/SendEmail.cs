using System.Net.Mail;
using TrueOrFalse;

public class SendEmail : IRegisterAsInstancePerLifetime
{
    public void Run(MailMessage mail)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Send(mail);
    }
}