using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class MiddlewareRefreshCookieController(SessionUser _sessionUser, PersistentLoginRepo _persistentLoginRepo, UserReadingRepo _userReadingRepo, IHttpContextAccessor _httpContextAccessor) : Controller

{
    [HttpGet]
    public void Get()
    {
        var cookieString = Request.Cookies[PersistentLoginCookie.Key];
        if (cookieString != null && _httpContextAccessor.HttpContext != null)
            LoginFromCookie.Run(_sessionUser, _persistentLoginRepo, _userReadingRepo, cookieString, _httpContextAccessor.HttpContext);
    }
}