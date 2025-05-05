using Microsoft.AspNetCore.Http;

public static class ErrorLogging
{
    public static void Log(Exception ex)
    {
        Serilog.Log.Error(ex, "Exception; {exceptionMessage}", ex.Message);
    }

    public static void Log(Exception ex, HttpContext ctx)
    {
        var req = ctx.Request;
        var url = $"{req.Path}{req.QueryString}";
        var headers = req.Headers.ToString();

        Serilog.Log.Error(ex, "PageError {Url} {Headers}", url, headers);
    }
}

public static class SubscriptionLogging
{
    public static void Info(StripePaymentEvents evt, int userId)
    {
        // Add a permanent property for Seq filtering
        var subLog = Log.ForContext("IsSubscription", true);

        switch (evt)
        {
            case StripePaymentEvents.Success:
                subLog.Information("Plan added by user {UserId}", userId);
                break;

            case StripePaymentEvents.Cancelled:
                subLog.Information("Plan cancelled by user {UserId}", userId);
                break;

            case StripePaymentEvents.Failed:
                subLog.Warning("Payment failed for user {UserId}", userId);
                break;
        }
    }
}