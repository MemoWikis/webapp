using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TrueOrFalse.Domain.User;

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
        var googleCredentialCookieString = httpContext.Request.Cookies[PersistentLoginCookie.GoogleCredential];
        var googleAccessTokenCookieString = httpContext.Request.Cookies[PersistentLoginCookie.GoogleAccessToken];

        if (persistentLoginCookieString != null || googleCredentialCookieString != null || googleAccessTokenCookieString != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();

                if (!sessionUser.IsLoggedIn)
                {
                    var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
                    var persistentLoginRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
                    var googleLogin = scope.ServiceProvider.GetRequiredService<GoogleLogin>();
                    try
                    {
                        if (persistentLoginCookieString != null)
                            LoginFromCookie.RunToRestore(sessionUser, persistentLoginRepo, userReadingRepo, persistentLoginCookieString);
                        else if (googleCredentialCookieString != null)
                            await LoginFromCookie.RunToRestoreGoogleCredential(sessionUser, userReadingRepo, googleLogin, googleCredentialCookieString);
                        else
                            await LoginFromCookie.RunToRestoreGoogleAccessToken(sessionUser, userReadingRepo, googleLogin, googleAccessTokenCookieString);
                    }
                    catch (Exception ex)
                    {
                        RemovePersistentLoginFromCookie.RunForGoogle(httpContext);
                        RemovePersistentLoginFromCookie.Run(persistentLoginRepo, httpContext);

                        Logg.Error(ex);
                    }
                }
            }
        }

        await _next(httpContext);
    }

}
