using Microsoft.AspNetCore.Http;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo, HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = false,
            Secure = false, // Set to true if using HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);


        httpContext.Response.Cookies.Delete(PersistentLoginCookie.Key);
        httpContext.Response.Cookies.Append(PersistentLoginCookie.Key, userId + "-x-" + loginGuid, cookieOptions);
    }        
}