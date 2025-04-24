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
        var environment = Settings.Environment;
        var url = "";
        if (!string.IsNullOrEmpty(Settings.StripeBaseUrl))
        {
            url = $"{Settings.StripeBaseUrl}/{targetPath}";
        }
        else if (environment.Equals("develop"))
        {
            url = $"http://localhost:3000/{targetPath}";
        }
        else
        {
            url = $"{Settings.BaseUrl}/{targetPath}";
        }

        return url;
    }
}