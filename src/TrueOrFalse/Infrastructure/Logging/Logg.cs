using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Rollbar;
using Serilog;
using TrueOrFalse.Infrastructure.Logging;
using TrueOrFalse.Tools;

public class Logg : IRegisterAsInstancePerLifetime
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string _seqUrl = "http://localhost:5341";
    private readonly Serilog.ILogger _logger;
    private readonly Serilog.ILogger _loggerIsCrawler;
    private readonly Serilog.ILogger _subscriptionLogger;
    public Logg(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;

        _logger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment",
                Settings.Environment(_httpContextAccessor.HttpContext, _webHostEnvironment))
            .Enrich.WithProperty("IsCrawler", false)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        _loggerIsCrawler = new LoggerConfiguration()
            .Enrich.WithProperty("Environment",
                Settings.Environment(_httpContextAccessor.HttpContext, _webHostEnvironment))
            .Enrich.WithProperty("IsCrawler", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();

        _subscriptionLogger = new LoggerConfiguration()
            .Enrich.WithProperty("Environment",
                Settings.Environment(_httpContextAccessor.HttpContext, _webHostEnvironment))
            .Enrich.WithProperty("isSubscription", true)
            .WriteTo.Seq(_seqUrl)
            .CreateLogger();



        //configure globally shared logger
        Log.Logger = _logger;
    }

    public void Error(Exception exception)
    {
        try
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                r().Error(exception, "Error");
                return;
            }

            var request = _httpContextAccessor.HttpContext.Request;
            var header = request.Headers.ToString();

            if (!IgnoreLog.ContainsCrawlerInHeader(header, _httpContextAccessor, _webHostEnvironment))
            {
                var rawUrl = $"{request.Path}{request.QueryString}";
                r(IsCrawlerRequest.Yes(_httpContextAccessor, _webHostEnvironment)).Error(exception, "PageError {Url} {Headers}",
                    rawUrl,
                    header);

                var connection = _httpContextAccessor.HttpContext.Connection;
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

    public Serilog.ILogger r(bool isCrawler = false)
    {
        return isCrawler ? _loggerIsCrawler : _logger;
    }

    /// <summary>
    ///     Log subscription events
    /// </summary>
    /// <param name="stripePaymentEvents"></param>
    /// <param name="userId"></param>
    public void SubscriptionLogger(StripePaymentEvents stripePaymentEvents, int userId)
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