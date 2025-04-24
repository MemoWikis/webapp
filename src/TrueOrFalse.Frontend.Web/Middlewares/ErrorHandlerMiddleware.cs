using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;


public class ErrorHandlerMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);

        if (httpContext.Response.StatusCode == 404)
        {
            Logg.r.Warning("404 Resource Not Found - {@Url}, {@Referer}", httpContext.Request.GetDisplayUrl(),
                httpContext.Request.Headers["Referer"]);
        }
        else if (httpContext.Response.StatusCode == 500)
        {
            Logg.Error(new Exception("Internal Error"));
        }
        else if (httpContext.Response.StatusCode == 503)
        {
            Logg.Error(new Exception("Server unavailable"));
        }
    }
}