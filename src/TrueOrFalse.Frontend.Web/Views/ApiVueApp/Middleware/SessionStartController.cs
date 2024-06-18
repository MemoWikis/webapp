using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VueApp;

public class MiddlewareSessionStartController(SessionUser _sessionUser, IServiceProvider _serviceProvider, HttpContext _httpContext) : Controller
{
    [HttpGet]
    public void Get()
    {
        var cookieValue = Request.Cookies[PersistentLoginCookie.Key];
        if (cookieValue != null)
        {
            // Autofac
            using (var scope = _serviceProvider.CreateScope())
            {
                var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();
                if (!sessionUser.IsLoggedIn)
                {
                    var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
                    var persistentLoggingRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
                    LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userReadingRepo, _httpContext);
                }
            }
        }
    }
}