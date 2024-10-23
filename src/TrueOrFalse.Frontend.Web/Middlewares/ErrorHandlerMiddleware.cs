using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;

namespace TrueOrFalse.Frontend.Web.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    Logg.r.Warning("404 Resource Not Found - {@Url}, {@Referer}",
                        httpContext.Request.GetDisplayUrl(),
                        httpContext.Request.Headers["Referer"]);
                }
                else if (httpContext.Response.StatusCode == 500)
                {
                    Logg.Error(new Exception("Internal Error"));
                }
                else if (httpContext.Response.StatusCode == 503)
                {
                    Logg.Error(new Exception("Server Unavailable"));
                }
            }
            catch (Exception ex)
            {
                Logg.Error(ex);

                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}