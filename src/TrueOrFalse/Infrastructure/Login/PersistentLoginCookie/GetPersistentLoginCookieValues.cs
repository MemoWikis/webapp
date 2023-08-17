using Microsoft.AspNetCore.Http;

public class GetPersistentLoginCookieValues
{
    public static GetPersistentLoginCookieValuesResult Run(IHttpContextAccessor httpContextAccessor)
    {
        var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies[Settings.MemuchoCookie];

        if (string.IsNullOrEmpty(cookieValue))
            return new GetPersistentLoginCookieValuesResult();

        var item = cookieValue.Split(new[] { "-x-" }, StringSplitOptions.None);

        if (item.Length < 2)
            return new GetPersistentLoginCookieValuesResult();

        return new GetPersistentLoginCookieValuesResult
        {
            UserId = Convert.ToInt32(item[0]),
            LoginGuid = item[1]
        };
    }
}