using System.Net.Mail;

public class SendEmail : IRegisterAsInstancePerLifetime
{
    public void Run(MailMessage mail)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Send(mail);
    }
}