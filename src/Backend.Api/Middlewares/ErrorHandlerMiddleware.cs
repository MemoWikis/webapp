using Microsoft.AspNetCore.Http.Extensions;


public class ErrorHandlerMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);

        if (httpContext.Response.StatusCode == 404)
        {
            Log.Warning("404 Resource Not Found - {@Url}, {@Referer}", httpContext.Request.GetDisplayUrl(),
                httpContext.Request.Headers["Referer"]);
        }
        else if (httpContext.Response.StatusCode == 500)
        {
            ErrorLogging.Log(new Exception("Internal Error"));
        }
        else if (httpContext.Response.StatusCode == 503)
        {
            ErrorLogging.Log(new Exception("Server unavailable"));
        }
    }
}