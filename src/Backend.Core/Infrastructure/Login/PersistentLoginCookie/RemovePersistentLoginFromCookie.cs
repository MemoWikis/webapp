using Microsoft.AspNetCore.Http;

public class RemovePersistentLoginFromCookie
{
    public static void Run(PersistentLoginRepo persistentLoginRepo, HttpContext httpContext)
    {
        var persistentCookieValue = PersistentLoginCookie.GetValues(httpContext);

        if (!persistentCookieValue.Exists())
            return;

        persistentLoginRepo.Delete(persistentCookieValue);

        var existingCookieValue = httpContext?.Request.Cookies[PersistentLoginCookie.Key];

        if (existingCookieValue != null)
            httpContext?.Response.Cookies.Delete(PersistentLoginCookie.Key);
    }

    public static void RunForGoogle(HttpContext httpContext)
    {
        var credentialCookieExists = httpContext?.Request.Cookies[PersistentLoginCookie.GoogleCredential];

        if (credentialCookieExists != null)
            httpContext?.Response.Cookies.Delete(PersistentLoginCookie.GoogleCredential);

        var accessTokenCookieExists = httpContext?.Request.Cookies[PersistentLoginCookie.GoogleAccessToken];

        if (accessTokenCookieExists != null)
            httpContext?.Response.Cookies.Delete(PersistentLoginCookie.GoogleAccessToken);
    }
}