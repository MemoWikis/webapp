using Microsoft.AspNetCore.Http;

public class PersistentLoginCookie
{
    public const string StringSeparator = "-x-";

    public static PersistentLoginCookieGetValuesResult GetValues(IHttpContextAccessor httpContextAccessor)
    {
        var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies[Settings.AuthCookieName];

        if (string.IsNullOrEmpty(cookieValue))
            return new PersistentLoginCookieGetValuesResult();

        var item = cookieValue.Split(new[] { StringSeparator }, StringSplitOptions.None);

        return new PersistentLoginCookieGetValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }
}