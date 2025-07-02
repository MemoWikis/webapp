using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;
using System.Text.Json;


public class ErrorHandlerMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            ErrorLogging.Log(ex);

            // Set response status to 500 if not already set
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";

                var details = "An unexpected error occurred. Please try again later."; // Generic message in production
                if (Debugger.IsAttached || Settings.Environment == "develop")
                {
                    details = ex.ToString();
                }

                var errorResponse = new
                {
                    error = "An internal server error occurred",
                    message = details
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }

            return; // Don't continue processing
        }

        if (httpContext.Response.StatusCode == 404)
        {
            Log.Warning("404 Resource Not Found - {@Url}, {@Referer}",
                httpContext.Request.GetDisplayUrl(),
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