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
                    var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
                    var persistentLoginRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
                    try
                    {
                        if (persistentLoginCookieString != null)
                            LoginFromCookie.RunToRestore(sessionUser, persistentLoginRepo, userReadingRepo, persistentLoginCookieString);
                        else
                            await LoginFromCookie.RunToRestoreGoogle(sessionUser, userReadingRepo, googleCredentialCookieString);
                    }
                    catch (Exception ex)
                    {
                        RemovePersistentLoginFromCookie.RunForGoogleCredentials(httpContext);
                        RemovePersistentLoginFromCookie.Run(persistentLoginRepo, httpContext);

                        Logg.Error(ex);
                    }
                }
            }
        }

        await _next(httpContext);
    }

}
