using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RazorLight;

public class HtmlMessage
{

    private RazorLightEngine? _engine;
    private JobQueueRepo _jobQueueRepo;
    private readonly UserReadingRepo _userReadingRepo;

    public HtmlMessage(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        JobQueueRepo jobQueueRepo,
        UserReadingRepo userReadingRepo)
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(new PathTo(httpContextAccessor, webHostEnvironment).EmailTemplate())
            .UseMemoryCachingProvider()
            .Build();
        _jobQueueRepo = jobQueueRepo;
        _userReadingRepo = userReadingRepo;
    }

    public async Task SendAsync(
        MailMessage2 mailMessage,
        string messageTitle,
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

        SendEmail.Run(mailMessage, _jobQueueRepo, _userReadingRepo);
    }
}