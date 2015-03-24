using System.Net.Mail;

public class SendBetaRequestEmail
{
    public static void Run(string requesterEmail)
    {
        Sl.R<SendEmail>().Run(new MailMessage(
            Settings.EmailFrom, 
            Settings.EmailTo,
            "Beta access request", 
            requesterEmail + " asked for private beta access."));
    }
}