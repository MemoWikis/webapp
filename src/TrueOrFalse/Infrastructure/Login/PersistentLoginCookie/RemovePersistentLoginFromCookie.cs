using Microsoft.AspNetCore.Http;
using System.Web;

public class RemovePersistentLoginFromCookie
{
    public static void Run(PersistentLoginRepo persistentLoginRepo, HttpContextAccessor httpContextAccessor)
    {
        var persistentCookieValue = GetPersistentLoginCookieValues.Run(httpContextAccessor);

        if (!persistentCookieValue.Exists())
            return;

        persistentLoginRepo.Delete(persistentCookieValue.UserId, persistentCookieValue.LoginGuid);

        var existingCookieValue = httpContextAccessor.HttpContext?.Request.Cookies[Settings.MemuchoCookie];
        if (existingCookieValue != null)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
            };

            httpContextAccessor.HttpContext?.Response.Cookies.Append(Settings.MemuchoCookie, "", cookieOptions);
        }
    }
}