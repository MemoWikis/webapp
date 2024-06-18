using Microsoft.AspNetCore.Http;

public class PersistentLoginCookie
{
    public const string Key = "persistentLogin";

    public static PersistentLoginCookieGetValuesResult GetValues(HttpContext httpContext)
    {
        var cookieValue = httpContext?.Request.Cookies[Key];

        if (string.IsNullOrEmpty(cookieValue))
            return new PersistentLoginCookieGetValuesResult();

        var item = cookieValue.Split(new[] { "-x-" }, StringSplitOptions.None);

        return new PersistentLoginCookieGetValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }

    public static PersistentLoginCookieGetValuesResult GetValues(string? cookieString)
    {
        if (string.IsNullOrEmpty(cookieString))
            return new PersistentLoginCookieGetValuesResult();

        var item = cookieString.Split(new[] { "-x-" }, StringSplitOptions.None);

        return new PersistentLoginCookieGetValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }
}