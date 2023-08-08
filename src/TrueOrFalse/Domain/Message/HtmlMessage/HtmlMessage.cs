

using System.Net.Mail;
using RazorLight;

public class HtmlMessage
{
    public static void Send(MailMessage2 mailMessage, 
        string messageTitle, 
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo, 
        string signOutMessage, 
        string utmSource)
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
                UtmCampaign = null
            });

        mailMessage.Body = parsedTemplate;
        mailMessage.IsBodyHtml = true;

        SendEmail.Run(mailMessage,jobQueueRepo, userReadingRepo);
    }

    public static async Task SendAsync(
        MailMessage2 mailMessage,
        string messageTitle,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo,
        string signOutMessage,
        string utmSource)
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(PathTo.EmailTemplate())
            .UseMemoryCachingProvider()
            .Build();

        string result = await engine.CompileRenderAsync("EmailTemplate.cshtml", new HtmlMessageModel
        {
            UserName = mailMessage.UserName,
            MessageTitle = messageTitle,
            Content = mailMessage.Body,
            SignOutMessage = signOutMessage,
            UtmSource = utmSource,
            UtmCampaign = null
        });

        mailMessage.Body = result;
        mailMessage.IsBodyHtml = true;

        SendEmail.Run(mailMessage, jobQueueRepo, userReadingRepo);
    }
}