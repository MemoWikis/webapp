using System.Net.Mail;

public class SendEmail
{
    public static void Run(MailMessage mail)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Send(mail);
    }
}