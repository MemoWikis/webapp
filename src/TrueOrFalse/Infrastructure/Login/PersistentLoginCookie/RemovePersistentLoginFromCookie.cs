using Microsoft.AspNetCore.Http;

public class RemovePersistentLoginFromCookie
{
    public static void Run(
        IHttpContextAccessor httpContextAccessor)
    {
        var persistentCookieValue = PersistentLoginCookie.GetValues(httpContextAccessor);

        if (!persistentCookieValue.Exists())
            return;

        var existingCookieValue =
            httpContextAccessor.HttpContext?.Request.Cookies[Settings.AuthCookieName];

        if (existingCookieValue != null)
            httpContextAccessor.HttpContext?.Response.Cookies.Delete(Settings.AuthCookieName);
    }
}