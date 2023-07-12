using System.IO;
using RazorEngine;

public class HtmlMessage
{
    public static void Send(MailMessage2 mailMessage, string messageTitle, JobQueueRepo jobQueueRepo,UserRepo userRepo, string signOutMessage = null, string utmSource = null, string utmCampaign = null)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate()),
            new HtmlMessageModel
            {
                UserName = mailMessage.UserName,
                MessageTitle = messageTitle,
                Content = mailMessage.Body,
                SignOutMessage = signOutMessage,
                UtmSource = utmSource,
                UtmCampaign = utmCampaign
            });

        mailMessage.Body = parsedTemplate;
        mailMessage.IsBodyHtml = true;

        SendEmail.Run(mailMessage,jobQueueRepo, userRepo);
    }
}