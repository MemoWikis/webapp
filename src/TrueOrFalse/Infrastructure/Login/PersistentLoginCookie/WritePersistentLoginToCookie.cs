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
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);
        httpContext.Response.Cookies.Append(PersistentLoginCookie.Key, userId + "-x-" + loginGuid, cookieOptions);
    }

    public record struct CookieResult(string LoginGuid, DateTimeOffset ExpiryDate);
    public static CookieResult Run(int userId, PersistentLoginRepo persistentLoginRepo)
    {
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);
        var expiryDate = DateTimeOffset.UtcNow.AddDays(30);

        return new CookieResult(userId + "-x-" + loginGuid, expiryDate);
    }
}