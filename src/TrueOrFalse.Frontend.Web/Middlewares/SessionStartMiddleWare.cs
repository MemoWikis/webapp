using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class SessionStartMiddleware
{
    public static readonly Object _lock = new object();
    private readonly RequestDelegate _next;

    public SessionStartMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IServiceProvider serviceProvider)
    {
        if (httpContext.Request.Headers.TryGetValue("X-Execute-Middleware", out var executeMiddlewareHeader) &&
            executeMiddlewareHeader == "true")
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
        }

        await _next(httpContext);
    }
}