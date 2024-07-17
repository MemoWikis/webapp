using Microsoft.AspNetCore.Http;
using Rollbar;
using Serilog;
using System.Net;
using TrueOrFalse.Infrastructure.Logging;
using TrueOrFalse.Tools;

public class Logg : IRegisterAsInstancePerLifetime
{
    private static readonly string _seqUrl = Settings.SeqUrl;
    private static readonly Serilog.ILogger _logger;
    private static readonly Serilog.ILogger _loggerIsCrawler;
    private static readonly Serilog.ILogger _subscriptionLogger;

    static Logg()
    {
        _logger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", App.Environment.EnvironmentName)
            .Enrich.WithProperty("IsCrawler", false)
            .WriteTo.Seq(_seqUrl, apiKey: Settings.SeqApiKey)
            .CreateLogger();

        _loggerIsCrawler = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", App.Environment.EnvironmentName)
            .Enrich.WithProperty("IsCrawler", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        _subscriptionLogger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment", App.Environment.EnvironmentName)
            .Enrich.WithProperty("isSubscription", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        //configure globally shared logger
        Log.Logger = _logger;
    }

    public static void Error(Exception exception) => r.Error(exception, "Error");

    public static void Error(Exception exception, HttpContext httpContext)
    {
        try
        {
            var request = httpContext.Request;
            var header = request.Headers.ToString();
            var rawUrl = $"{request.Path}{request.QueryString}";

            r.Error(exception, "PageError {Url} {Headers}", rawUrl, header);

            if (!IgnoreLog.ContainsCrawlerInHeader(header))
            {
                var connection = httpContext.Connection;
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

    // ReSharper disable once InconsistentNaming
    public static Serilog.ILogger r => _logger;

    /// <summary>
    /// Log subscription events
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