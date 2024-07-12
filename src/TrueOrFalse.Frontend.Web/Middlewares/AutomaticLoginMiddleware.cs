using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueOrFalse.Frontend.Web.Middlewares
{
    public class AutomaticLoginMiddleware(RequestDelegate _next, IServiceProvider _serviceProvider)
    {
        private readonly HashSet<string> _excludedPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/apiVue/App/SessionStart",
            "/apiVue/App/RenewPersistentCookie" // Add your other paths here
        };

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (_excludedPaths.Contains(httpContext.Request.Path))
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
                            LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userReadingRepo, cookieString);
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
}
