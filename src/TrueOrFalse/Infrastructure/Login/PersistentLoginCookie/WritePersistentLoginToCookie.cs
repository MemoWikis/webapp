using Microsoft.AspNetCore.Http;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, IHttpContextAccessor httpContextAccessor)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365),
            IsEssential = true,
        };
        var encryptUserId = CreatePersistentLogin.EncryptUserId(userId);
        var encryptDate = CreatePersistentLogin.EncryptDate();

        httpContextAccessor.HttpContext.Response.Cookies.Delete(Settings.AuthCookieName);
        httpContextAccessor.HttpContext.Response.Cookies.Append(Settings.AuthCookieName,
            encryptUserId + PersistentLoginCookie.StringSeparator + encryptDate);
    }        
}