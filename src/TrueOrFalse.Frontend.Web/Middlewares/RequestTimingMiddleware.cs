using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TrueOrFalse.View.Web.Middlewares;

/// <summary>
///     Middleware that measures the processing time for <b>non‑static</b> HTTP requests (i.e. URLs
///     without a file extension) and logs the elapsed duration <b>only in the Development
///     environment</b>. 
/// </summary>
public sealed class RequestTimingMiddleware(
    RequestDelegate _next,
    ILogger<RequestTimingMiddleware> _logger,
    IWebHostEnvironment _environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Only log in development environment
        if (!_environment.IsDevelopment())
        {
            await _next(context);
            return;
        }

        // Skip static files (have a file extension)
        if (Path.HasExtension(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("=== Start Request: {PathAndQuery} ===", $"{context.Request.Path}{context.Request.QueryString}");

        await _next(context);

        _logger.LogInformation("=== End Request: {PathAndQuery} – {Elapsed} ===", $"{context.Request.Path}{context.Request.QueryString}", stopwatch.Elapsed);
    }
}