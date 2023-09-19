using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RequestTimingForStaticFilesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public RequestTimingForStaticFilesMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _contextAccessor = contextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = new Stopwatch();

#if DEBUG
        //if (Settings.DebugEnableMiniProfiler())
        //MiniProfiler.Current.Stop();

        if (!_contextAccessor.HttpContext.Request.Path.Value.Contains("."))
        {
            context.Items.Add("requestStopwatch", Stopwatch.StartNew());
            Logg.r.Information("=== Start Request: {pathAndQuery} ==", context.Request.Path.Value);
        }
#endif

        await _next(context);

#if DEBUG
        if (_contextAccessor.HttpContext.Items.ContainsKey("requestStopwatch"))
        {
            stopwatch = _contextAccessor.HttpContext.Items["requestStopwatch"] as Stopwatch;
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            Logg.r.Information("=== End Request: {pathAndQuery} {elapsed}==", _contextAccessor.HttpContext.Request.Path + _contextAccessor.HttpContext.Request.QueryString, elapsed);
        }
#endif
    }
}