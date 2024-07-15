using Microsoft.AspNetCore.Mvc;
using System;

namespace VueApp;

public class MiddlewareRefreshCookieController(SessionUser _sessionUser, PersistentLoginRepo _persistentLoginRepo, UserReadingRepo _userReadingRepo) : Controller

{
    public record struct GetResponse(bool success, string? loginGuid = null, DateTimeOffset? expiryDate = null, bool alreadyLoggedIn = false);

    [HttpGet]
    public GetResponse Get()
    {
        var cookieString = Request.Cookies[PersistentLoginCookie.Key];
        if (cookieString != null)
        {
            var loginResult = LoginFromCookie.Run(_sessionUser, _persistentLoginRepo, _userReadingRepo, cookieString);
            if (loginResult.Success)
                return new GetResponse(true, loginResult.LoginGuid, loginResult.ExpiryDate);
        }

        return new GetResponse(false);
    }
}