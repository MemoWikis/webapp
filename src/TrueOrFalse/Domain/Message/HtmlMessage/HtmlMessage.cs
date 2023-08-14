using RazorLight;

public static class HtmlMessage
{

    private static RazorLightEngine? _engine;

    static HtmlMessage()
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(PathTo.EmailTemplate())
            .UseMemoryCachingProvider()
            .Build();
    }
    public static async Task Send(MailMessage2 mailMessage, 
        string messageTitle, 
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo, 
        string signOutMessage, 
        string utmSource)
    {
        string result = await _engine?.CompileRenderAsync("EmailTemplate.cshtml", new HtmlMessageModel
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

    public static async Task SendAsync(
        MailMessage2 mailMessage,
        string messageTitle,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo,
        string signOutMessage,
        string utmSource)
    {
        string result = await _engine?.CompileRenderAsync("EmailTemplate.cshtml", new HtmlMessageModel
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