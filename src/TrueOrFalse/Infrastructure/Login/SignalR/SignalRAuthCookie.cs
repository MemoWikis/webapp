using System;
using System.Web;

public class SignalRAuthCookie
{
    public static void WriteToken(string authToken)
    {
        var cookie = MemuchoCookie.GetNew();
        cookie.Values.Add("SignalR_Auth", authToken);

        HttpContext.Current.Response.Cookies.Add(cookie);
    }
}