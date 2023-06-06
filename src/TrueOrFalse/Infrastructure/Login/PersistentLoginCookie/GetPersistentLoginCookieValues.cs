using System.Web;

public class GetPersistentLoginCookieValues
{
    public static GetPersistentLoginCookieValuesResult Run()
    {
        var cookie = HttpContext.Current.Request.Cookies.Get(Settings.MemuchoCookie);

        if (cookie == null)
            return new GetPersistentLoginCookieValuesResult();

        var valuePersistentLogin = cookie["persistentLogin"];
        if (string.IsNullOrEmpty(valuePersistentLogin))
            return new GetPersistentLoginCookieValuesResult();

        var item = valuePersistentLogin.Split(new[] { "-x-" }, StringSplitOptions.None);

        return new GetPersistentLoginCookieValuesResult
                    {
                        UserId = Convert.ToInt32(item[0]),
                        LoginGuid = item[1]
                    };
    }
}