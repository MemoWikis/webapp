using Microsoft.AspNetCore.Http;

public class SessionActivityMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Session?.GetInt32("userId") > 0)
        {
            LoggedInSessionStore.TouchLoggedInUsers(httpContext.Session.Id);
        }
        else
        {
            LoggedInSessionStore.TouchOrAddAnonymousUsers(httpContext.Session.Id);
        }

        await _next(httpContext);
    }
}

