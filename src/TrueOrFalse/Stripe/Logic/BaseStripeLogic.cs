using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Lib.Settings;


public class BaseStripeLogic
{
    protected static string CreateSiteLink(string targetPath,
        HttpContext httpContext, 
        IWebHostEnvironment webHostEnvironment)
    {
        var server = Settings.Environment(httpContext, webHostEnvironment);
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