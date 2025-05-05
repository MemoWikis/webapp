public class StripeReturnUrlGenerator
{
    public static string Create(string targetPath)
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