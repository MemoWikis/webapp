using System;
using System.Web;
using System.Web.Caching;

public class SignalRAuthTokenRepo
{

    public static void Insert(SignalRAuthInfo signalRAuthInfo)
    {
        new Cache().Insert(
            signalRAuthInfo.CookieToken, 
            signalRAuthInfo, 
            null /* Cache dependency */, 
            Cache.NoAbsoluteExpiration, 
            TimeSpan.FromHours(1));
    }

    public static SignalRAuthInfo Get(string idToken)
    {
        var result = HttpContext.Current.Cache.Get(idToken);
        if (result == null)
            return null;

        return (SignalRAuthInfo) (result);
    }

    public static void Remove(string idToken)
    {
        var result = HttpContext.Current.Cache.Get(idToken);
        if (result == null)
            return;

        HttpContext.Current.Cache.Remove(idToken);
    }
}