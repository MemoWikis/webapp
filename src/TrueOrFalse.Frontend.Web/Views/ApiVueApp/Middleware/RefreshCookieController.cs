using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace VueApp;

public class MiddlewareRefreshCookieController(SessionUser _sessionUser, PersistentLoginRepo _persistentLoginRepo, UserReadingRepo _userReadingRepo, IHttpContextAccessor _httpContextAccessor) : Controller
{
    public readonly record struct RunResponse(bool Success, bool? DeleteCookie = null);

    [HttpGet]
    public RunResponse Run() => TryRun();

    private RunResponse TryRun()
    {
        try
        {
            LoginFromCookie.Run(_sessionUser, _persistentLoginRepo, _userReadingRepo, _httpContextAccessor.HttpContext);
            return new RunResponse(true);
        }
        catch (Exception ex)
        {
            if (ex.Data.Contains("InvalidCookie") && (bool)ex.Data["InvalidCookie"])
            {
                return new RunResponse(false, true);
            }
            return new RunResponse(false);
        }
    }
}