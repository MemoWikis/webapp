using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class SessionStartMiddleware
{
    private readonly RequestDelegate _next;

    public SessionStartMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IServiceProvider serviceProvider)
    {
        var cookieValue = httpContext?.Request.Cookies[PersistentLoginCookie.Key];
        if (cookieValue != null)
        {
            // Autofac
            using (var scope = serviceProvider.CreateScope())
            {
                var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();
                if (!sessionUser.IsLoggedIn)
                {
                    var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
                    var persistentLoggingRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();
                    LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userReadingRepo, httpContext);
                }
            }
        }

        await _next(httpContext);
    }
}