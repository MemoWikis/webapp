public class SessionStartMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SessionStartMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        // Ihre Logik für Session_Start
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var referrer = context.Request.Headers["Referer"].ToString() ?? "No referrer";
        new Logg(_httpContextAccessor, _webHostEnvironment).r().Information("SessionStart - userAgent: {userAgent}, referrer: {referrer}", userAgent, referrer);

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