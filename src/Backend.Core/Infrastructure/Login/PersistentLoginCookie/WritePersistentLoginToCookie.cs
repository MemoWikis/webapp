using Microsoft.AspNetCore.Http;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo, HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            Path = "/",
            HttpOnly = true,
            Secure = Settings.Environment != "develop", // Set to true if using HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(180)
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);
        httpContext.Response.Cookies.Append(PersistentLoginCookie.Key, userId + "-x-" + loginGuid, cookieOptions);
    }
}