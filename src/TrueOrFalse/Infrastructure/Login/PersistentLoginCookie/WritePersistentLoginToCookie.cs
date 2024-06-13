using Microsoft.AspNetCore.Http;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo, HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(7)
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);


        httpContext.Response.Cookies.Delete(PersistentLoginCookie.Key);
        httpContext.Response.Cookies.Append(PersistentLoginCookie.Key, userId + "-x-" + loginGuid, cookieOptions);
    }        
}