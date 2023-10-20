using Microsoft.AspNetCore.Http;

public class GetPersistentLoginCookieValues
{
    public static GetPersistentLoginCookieValuesResult Run(IHttpContextAccessor httpContextAccessor)
    {
        var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies[Settings.PersistentLogin];

        if (string.IsNullOrEmpty(cookieValue))
            return new GetPersistentLoginCookieValuesResult();

        var item = cookieValue.Split(new[] { "-x-" }, StringSplitOptions.None);

        return new GetPersistentLoginCookieValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }
}