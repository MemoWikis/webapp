using Microsoft.AspNetCore.Http;

public class SessionActivityMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Session?.GetInt32("userId") > 0)
        {
            LoggedInSessionStore.Touch(httpContext.Session.Id);
        }

        await _next(httpContext);
    }
}

