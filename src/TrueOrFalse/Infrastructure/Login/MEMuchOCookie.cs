using System.Web;

public class MemuchoCookie
{
    public static HttpCookie GetNew()
    {
        var cookie = new HttpCookie(Settings.MemuchoCookie);
        cookie.Expires = DateTime.Now.AddDays(45);

        return cookie;
    }
}
