using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TrueOrFalse.Frontend.Web.Middlewares
{
    public class AutomaticLoginMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var excludedPath = "/apiVue/App/RenewPersistentCookie";

            if (httpContext.Request.Path.Equals(excludedPath, StringComparison.OrdinalIgnoreCase))
            {
                await next(httpContext);
                return;
            }

            var cookieString = httpContext.Request.Cookies[PersistentLoginCookie.Key];

            if (cookieString != null)
            {
                using (var scope = serviceProvider.CreateScope())
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

            await next(httpContext);
        }
    }
}