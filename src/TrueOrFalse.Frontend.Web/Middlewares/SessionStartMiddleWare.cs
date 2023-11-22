using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class SessionStartMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionStartMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        var cookieValue = _httpContextAccessor.HttpContext?.Request.Cookies[PersistentLoginCookie.Key];
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

                    LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userReadingRepo, _httpContextAccessor);
                }
            }
        }

        await _next(context);
    }
}