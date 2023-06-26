using System.Web;
using RollbarSharp;
using Serilog;
using TrueOrFalse.Infrastructure.Logging;
using TrueOrFalse.Tools;

public class Logg
{
    private const string _seqUrl = "http://localhost:5341";
    private static readonly ILogger _logger;
    private static readonly ILogger _loggerIsCrawler;
    private static readonly ILogger _subscriptionLogger;

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

        _subscriptionLogger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", Settings.Environment())
            .Enrich.WithProperty("isSubscription", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();


        //configure globally shared logger
        Log.Logger = _logger;
    }


    public static void Error(Exception exception)
    {
        try
        {
            if (HttpContext.Current == null)
            {
                r().Error(exception, "Error");
                return;
            }

            var request = HttpContext.Current.Request;
            var header = request.Headers.ToString();

            if (!IgnoreLog.ContainsCrawlerInHeader(header))
            {
                r(IsCrawlerRequest.Yes(UserAgent.Get())).Error(exception, "PageError {Url} {Headers}",
                    request.RawUrl,
                    header);

                if (!request.IsLocal)
                {
                    new RollbarClient().SendException(exception);
                }
            }
        }
        catch
        {
        }
    }

    public static ILogger r(bool isCrawler = false)
    {
        return isCrawler ? _loggerIsCrawler : _logger;
    }

    /// <summary>
    ///     Log subscription events
    /// </summary>
    /// <param name="stripePaymentEvents"></param>
    /// <param name="userId"></param>
    public static void SubscriptionLogger(StripePaymentEvents stripePaymentEvents, int userId)
    {
        if (userId == -1)
        {
            _subscriptionLogger.Error(new NullReferenceException("SessionUser null"), "SessionUserLegacy null");
            return;
        }

        if (stripePaymentEvents == StripePaymentEvents.Success)
        {
            _subscriptionLogger.Information($"Plan added from User{userId}");
        }
        else if (stripePaymentEvents == StripePaymentEvents.Cancelled)
        {
            _subscriptionLogger.Information($"Plan cancelled from User{userId}");
        }
        else if (stripePaymentEvents == StripePaymentEvents.Failed)
        {
            _subscriptionLogger.Information($"PaymentFailed from User{userId}");
        }
    }
}