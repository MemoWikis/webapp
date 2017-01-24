using System.Web;


public class UserAgent
{
    public static string Get()
    {
        string userAgent = "";
        if (HttpContext.Current.Request.UserAgent != null)
            userAgent = HttpContext.Current.Request.UserAgent.ToLower();

        return userAgent;
    }
}