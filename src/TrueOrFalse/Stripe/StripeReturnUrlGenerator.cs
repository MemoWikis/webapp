using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


public class StripeReturnUrlGenerator
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IWebHostEnvironment _webHostEnvironment;

    public StripeReturnUrlGenerator(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public string Create(string targetPath)
    {
        var server = Settings.Environment;
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