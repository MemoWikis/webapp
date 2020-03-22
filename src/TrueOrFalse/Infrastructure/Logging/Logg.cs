using System;
using System.Web;
using RollbarSharp;
using Serilog;
using TrueOrFalse.Tools;

public class Logg
{
    private static ILogger _logger;
    //private static bool isCrawler; 

    public static ILogger r(bool isCrawler = false)
    {
        _logger = new LoggerConfiguration()
                .Enrich.WithProperty("Environment", Settings.Environment())
                .Enrich.WithProperty("isCrawler", isCrawler)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

        return _logger;
    }

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
            var _isCrawler = IsCrawlerRequest.Yes(UserAgent.Get()); 

            if (!IgnoreLog.ContainsCrawlerInHeader(header))
            {
                Logg.r(_isCrawler).Error(exception, "PageError {Url} {Headers}",
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