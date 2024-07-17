using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TrueOrFalse.Frontend.Web.Middlewares;

public class AutoLoginMiddleware(RequestDelegate _next, IServiceProvider _serviceProvider)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var excludedPath = "/apiVue/MiddlewareRefreshCookie/Get";

        if (httpContext.Request.Path.Equals(excludedPath, StringComparison.OrdinalIgnoreCase))
        {
            await _next(httpContext);
            return;
        }

        var persistentLoginCookieString = httpContext.Request.Cookies[PersistentLoginCookie.Key];
        var googleCredentialCookieString = httpContext.Request.Cookies[PersistentLoginCookie.GoogleKey];

        if (persistentLoginCookieString != null || googleCredentialCookieString != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();
                if (!sessionUser.IsLoggedIn)
                {
                    if (persistentLoginCookieString != null)
                        TryLoginFromPersistenLoginCookie(scope, sessionUser, persistentLoginCookieString);
                    else
                        await LoginFromCookie.RunToRestoreGoogle(sessionUser, scope.ServiceProvider.GetRequiredService<UserReadingRepo>(), googleCredentialCookieString);

                }
            }
        }

        await _next(httpContext);
    }

    private void TryLoginFromPersistenLoginCookie(IServiceScope scope, SessionUser sessionUser, string cookieString)
    {
        var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
        var persistentLoginRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
        try
        {
            LoginFromCookie.RunToRestore(sessionUser, persistentLoginRepo, userReadingRepo, cookieString);
        }
        catch (Exception ex)
        {
            Logg.Error(ex);
        }
    }
}
