using System.Web;

public class RemovePersistentLoginFromCookie
{
    public static void Run(PersistentLoginRepo persistentLoginRepo)
    {
        var persistentCookieValue = GetPersistentLoginCookieValues.Run();

        if (!persistentCookieValue.Exists())
            return;

        persistentLoginRepo.Delete(persistentCookieValue.UserId, persistentCookieValue.LoginGuid);
        var cookie = HttpContext.Current.Response.Cookies.Get(Settings.MemuchoCookie);
        cookie.Values.Set("persistentLogin", "");
        cookie.Expires = DateTime.Now.AddDays(45);
    }
}