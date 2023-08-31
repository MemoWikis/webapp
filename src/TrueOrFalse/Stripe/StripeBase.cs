using System;
using Seedworks.Lib.Settings;


public class StripeBase
{
    protected static string CreateSiteLink(string targetPath)
    {
        var server = Settings.Environment();
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