namespace TrueOrFalse.Stripe.Logic;

public class BaseStripeLogic
{
    protected string CreateSiteLink(string targetPath)
    {
        var server = Settings.Environment();
        var url = "";
        if (server.Equals("develop"))
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