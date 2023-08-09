using System.Net;
using System.Web;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Ubiety.Dns.Core;

public class WritePersistentLoginToCookie
{
    public static void Run(int userId, PersistentLoginRepo persistentLoginRepo, IHttpContextAccessor httpContextAccessor)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(45)
        };
        var loginGuid = CreatePersistentLogin.Run(userId, persistentLoginRepo);

        httpContextAccessor.HttpContext?.Response.Cookies.Append("persistentLogin", userId + "-x-" + loginGuid, cookieOptions);

    }        
}