using System.Net.Mail;

public class SendBetaRequestEmail
{
    public static void Run(string requesterEmail)
    {
        SendEmail.Run(new MailMessage(
            Settings.EmailFrom, 
            Settings.EmailToMemucho,
            "Beta access request", 
            requesterEmail + " asked for private beta access."), MailMessagePriority.Low);
    }
}