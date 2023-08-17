using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


public class BaseStripeLogic
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IWebHostEnvironment _webHostEnvironment;

    public BaseStripeLogic(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    protected string CreateSiteLink(string targetPath)
    {
        var server = Settings.Environment(_httpContextAccessor.HttpContext, _webHostEnvironment);
        var url = "";
        if (!string.IsNullOrEmpty(Settings.StripeBaseUrl))
        {
            url = $"{Settings.StripeBaseUrl}/{targetPath}";
        }
        else if (server.Equals("develop"))
        {
            url = $"http://localhost:3000/{targetPath}";
        }
        else if (server.Equals("stage"))
        {
            url = $"https://stage.memucho.de/{targetPath}";
        }
        else
        {
            url = $"https://memucho.de/{targetPath}";
        }

        return url;
    }
}