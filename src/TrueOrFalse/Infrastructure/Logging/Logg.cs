using System.Net;
using Microsoft.AspNetCore.Http;
using Rollbar;
using Serilog;
using TrueOrFalse.Infrastructure.Logging;
using TrueOrFalse.Tools;

public class Logg
{
    private const string _seqUrl = "http://localhost:5341";
    private static readonly Serilog.ILogger _logger;
    private static readonly Serilog.ILogger _loggerIsCrawler;
    private static readonly Serilog.ILogger _subscriptionLogger;

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


    public static void Error(Exception exception, IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            if (httpContextAccessor.HttpContext == null)
            {
                r().Error(exception, "Error");
                return;
            }

            var request = httpContextAccessor.HttpContext.Request;
            var header = request.Headers.ToString();

            if (!IgnoreLog.ContainsCrawlerInHeader(header))
            {
                var rawUrl = $"{request.Path}{request.QueryString}";
                r(IsCrawlerRequest.Yes(UserAgent.Get())).Error(exception, "PageError {Url} {Headers}",
                    rawUrl,
                    header);

                var connection = httpContextAccessor.HttpContext.Connection;
                if (connection.RemoteIpAddress.Equals(connection.LocalIpAddress) || IPAddress.IsLoopback(connection.RemoteIpAddress))
                {
                    RollbarLocator.RollbarInstance.Error(new Rollbar.DTOs.Body(exception));
                }
            }
        }
        catch
        {
        }
    }

    public static Serilog.ILogger r(bool isCrawler = false)
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
            _subscriptionLogger.Error(new NullReferenceException("SessionUser null"), "SessionUser null");
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