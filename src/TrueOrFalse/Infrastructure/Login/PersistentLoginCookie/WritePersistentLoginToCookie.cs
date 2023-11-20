using Microsoft.AspNetCore.Http;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo, IHttpContextAccessor httpContextAccessor)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(45),
            IsEssential = true,
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);

        httpContextAccessor.HttpContext.Response.Cookies.Delete(PersistentLoginCookie.Key);
        httpContextAccessor.HttpContext.Response.Cookies.Append(PersistentLoginCookie.Key, userId + "-x-" + loginGuid, cookieOptions);

    }        
}