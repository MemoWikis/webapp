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
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var referrer = context.Request.Headers["Referer"].ToString() ?? "No referrer";
        Logg.r.Information("SessionStart - userAgent: {userAgent}, referrer: {referrer}", userAgent, referrer);

        // Autofac
        using (var scope = serviceProvider.CreateScope())
        {
            var sessionUser = scope.ServiceProvider.GetRequiredService<SessionUser>();
            var userReadingRepo = scope.ServiceProvider.GetRequiredService<UserReadingRepo>();
            var persistentLoggingRepo = scope.ServiceProvider.GetRequiredService<PersistentLoginRepo>();

            if (!sessionUser.IsLoggedIn)
            {
                LoginFromCookie.Run(sessionUser, persistentLoggingRepo, userReadingRepo, _httpContextAccessor);
            }
        }

        await _next(context);
    }
}