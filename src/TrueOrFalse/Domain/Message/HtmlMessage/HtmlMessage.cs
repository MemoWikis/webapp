using System.IO;
using RazorEngine;

public class HtmlMessage
{
    public static void Send(MailMessage2 mailMessage, string utmSource = null, string utmCampaign = null)
    {
        var parsedTemplate = Razor.Parse(
            File.ReadAllText(PathTo.EmailTemplate()),
            new HtmlMessageModel
            {
                UserName = mailMessage.UserName,
                Content = mailMessage.Body,
                UtmSource = utmSource,
                UtmCampaign = utmCampaign
            });

        mailMessage.Body = parsedTemplate;
        mailMessage.IsBodyHtml = true;

        SendEmail.Run(mailMessage);
    }
}