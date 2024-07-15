using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TrueOrFalse.Frontend.Web.Middlewares;

public class AutomaticLoginMiddleware(RequestDelegate _next, IServiceProvider _serviceProvider)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var excludedPath = "/apiVue/MiddlewareRefreshCookie/Get";

        if (httpContext.Request.Path.Equals(excludedPath, StringComparison.OrdinalIgnoreCase))
        {
            await _next(httpContext);
            return;
        }

        var cookieString = httpContext.Request.Cookies[PersistentLoginCookie.Key];

        if (cookieString != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();
                if (!sessionUser.IsLoggedIn)
                {
                    var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
                    var persistentLoggingRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
                    try
                    {
                        LoginFromCookie.RunToRestore(sessionUser, persistentLoggingRepo, userReadingRepo, cookieString);
                    }
                    catch (Exception ex)
                    {
                        Logg.Error(ex);
                    }
                }
            }
        }

        await _next(httpContext);
    }
}
