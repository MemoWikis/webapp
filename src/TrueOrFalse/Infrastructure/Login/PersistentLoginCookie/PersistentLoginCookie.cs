using Microsoft.AspNetCore.Http;

public class PersistentLoginCookie
{
    public const string Key = "persistentLogin";
    
    public static PersistentLoginCookieGetValuesResult GetValues(IHttpContextAccessor httpContextAccessor)
    {
        var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies[Key];

        if (string.IsNullOrEmpty(cookieValue))
            return new PersistentLoginCookieGetValuesResult();

        var item = cookieValue.Split(new[] { "-x-" }, StringSplitOptions.None);

        return new PersistentLoginCookieGetValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }
}