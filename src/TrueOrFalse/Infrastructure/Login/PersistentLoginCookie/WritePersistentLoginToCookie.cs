using System.Web;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo)
    {
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);

        var cookie = MemuchoCookie.GetNew();
        cookie.Values.Add("persistentLogin", userId + "-x-" + loginGuid);
        HttpContext.Current.Response.Cookies.Add(cookie);
    }        
}