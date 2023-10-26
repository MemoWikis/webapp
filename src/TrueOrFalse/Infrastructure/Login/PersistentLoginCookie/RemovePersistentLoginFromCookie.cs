using Microsoft.AspNetCore.Http;
using System.Web;

public class RemovePersistentLoginFromCookie
{
    public static void Run(PersistentLoginRepo persistentLoginRepo, IHttpContextAccessor httpContextAccessor)
    {
        var persistentCookieValue = PersistentLoginCookie.GetValues(httpContextAccessor);

        if (!persistentCookieValue.Exists())
            return;

        persistentLoginRepo.Delete(persistentCookieValue.UserId);

        var existingCookieValue = httpContextAccessor.HttpContext?.Request.Cookies[PersistentLoginCookie.Key];
        if (existingCookieValue != null)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete(PersistentLoginCookie.Key);
        }
    }
}