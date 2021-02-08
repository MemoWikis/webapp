using System;
using System.Web;
using RollbarSharp;
using Serilog;
using TrueOrFalse.Tools;

public class Logg
{
    private static readonly ILogger _logger;
    private static readonly ILogger _loggerIsCrawler;

    private const string _seqUrl = "http://localhost:5341";

    static Logg()
    {
        _logger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", Settings.Environment())
            .Enrich.WithProperty("IsCrawler", false)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        _loggerIsCrawler = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", Settings.Environment())
            .Enrich.WithProperty("IsCrawler", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        //configure globally shared logger
        Log.Logger = _logger;
    }

    public static ILogger r(bool isCrawler = false) => isCrawler ? _loggerIsCrawler : _logger;

    public static void Error(Exception exception)
    {
        try
        {
            if (HttpContext.Current == null)
            {
                Logg.r().Error(exception, "Error");
                return;
            }
            
            var request = HttpContext.Current.Request;
            var header = request.Headers.ToString();
            
            if (!IgnoreLog.ContainsCrawlerInHeader(header))
            {
                Logg.r(IsCrawlerRequest.Yes(UserAgent.Get())).Error(exception, "PageError {Url} {Headers}",
                    request.RawUrl,
                    header);

                if (!request.IsLocal)
                    new RollbarClient().SendException(exception);
            }
        }
        catch
        {

        }

    }
}