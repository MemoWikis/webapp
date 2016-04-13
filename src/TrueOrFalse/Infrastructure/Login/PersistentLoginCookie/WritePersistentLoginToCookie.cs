using System.Web;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId)
    {
        var loginGuid = CreatePersistentLogin.Run(userId);

        var cookie = MemuchoCookie.GetNew();
        cookie.Values.Add("persistentLogin", userId + "-x-" + loginGuid);
        HttpContext.Current.Response.Cookies.Add(cookie);
    }        
}