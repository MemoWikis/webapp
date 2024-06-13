using Microsoft.AspNetCore.Http;

public class RemovePersistentLoginFromCookie
{
    public static void Run(
        PersistentLoginRepo persistentLoginRepo,
        HttpContext httpContext)
    {
        var persistentCookieValue = PersistentLoginCookie.GetValues(httpContext);

        if (!persistentCookieValue.Exists())
            return;

        persistentLoginRepo.Delete(persistentCookieValue.UserId);

        var existingCookieValue =
            httpContext?.Request.Cookies[PersistentLoginCookie.Key];
        if (existingCookieValue != null)
        {
            httpContext?.Response.Cookies.Delete(PersistentLoginCookie.Key);
        }
    }
}