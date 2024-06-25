using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RequestTimingForStaticFilesMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingForStaticFilesMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = new Stopwatch();

#if DEBUG
        //if (Settings.DebugEnableMiniProfiler())
        //MiniProfiler.Current.Stop();

        if (!context.Request.Path.Value.Contains("."))
        {
            context.Items.Add("requestStopwatch", Stopwatch.StartNew());
            Logg.r.Information("=== Start Request: {pathAndQuery} ==", context.Request.Path.Value);
        }
#endif

        await _next(context);

#if DEBUG
        if (context.Items.ContainsKey("requestStopwatch"))
        {
            stopwatch = context.Items["requestStopwatch"] as Stopwatch;
            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            Logg.r.Information("=== End Request: {pathAndQuery} {elapsed}==",
                context.Request.Path +
                context.Request.QueryString, elapsed);
        }
#endif
    }
}