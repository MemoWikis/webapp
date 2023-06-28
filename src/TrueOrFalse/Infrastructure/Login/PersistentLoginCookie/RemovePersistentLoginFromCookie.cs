using System.Web;

public class RemovePersistentLoginFromCookie
{
    public static void Run()
    {
        var persistentCookieValue = GetPersistentLoginCookieValues.Run();

        if (!persistentCookieValue.Exists())
            return;

        Sl.R<PersistentLoginRepo>().Delete(persistentCookieValue.UserId, persistentCookieValue.LoginGuid);
        var cookie = HttpContext.Current.Response.Cookies.Get(Settings.MemuchoCookie);
        cookie.Values.Set("persistentLogin", "");
        cookie.Expires = DateTime.Now.AddDays(45);
    }
}