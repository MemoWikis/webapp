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

            // Set response status based on exception type
            if (!httpContext.Response.HasStarted)
            {
                var (statusCode, errorMessage) = GetStatusCodeAndMessage(ex);
                
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = "application/json";

                var details = errorMessage; // Use specific message or generic in production
                if (Debugger.IsAttached || Settings.Environment == "develop")
                {
                    details = ex.ToString();
                }

                var errorResponse = new
                {
                    error = GetErrorTitle(statusCode),
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

    private static (int statusCode, string errorMessage) GetStatusCodeAndMessage(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException => (401, "Access denied. You are not authorized to perform this action."),
            FileNotFoundException => (404, "The requested resource was not found."),
            _ => (500, "An unexpected error occurred. Please try again later.")
        };
    }

    private static string GetErrorTitle(int statusCode)
    {
        return statusCode switch
        {
            401 => "Unauthorized",
            404 => "Not Found",
            500 => "Internal Server Error",
            503 => "Service Unavailable",
            _ => "Error"
        };
    }
}