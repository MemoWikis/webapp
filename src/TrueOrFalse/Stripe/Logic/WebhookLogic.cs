using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TrueOrFalse.Infrastructure.Logging;

public class WebhookLogic : IRegisterAsInstancePerLifetime
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DateTime MaxValueMysql = new(9999, 12, 31, 23, 59, 59);

    public WebhookLogic(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _httpContextAccessor = httpContextAccessor;
    }
    //public async Task<HttpStatusCodeResult> Create(HttpContextBase context, HttpRequestBase baseRequest)
    //{
    //    var eventAndStatus = await GetEvent(context, baseRequest);
    //    var stripeEvent = eventAndStatus.stripeEvent;
    //    var status = eventAndStatus.httpStatusCode;
    //    return Evaluate(stripeEvent, status);
    //}
    public async Task<IActionResult> Create()
    {
        var eventAndStatus = await GetEvent(_httpContextAccessor.HttpContext.Request);
        var stripeEvent = eventAndStatus.stripeEvent;

        var c = eventAndStatus.httpResult;
        return Evaluate(stripeEvent, eventAndStatus.httpResult);
    }
    public IActionResult Evaluate(Event stripeEvent, IActionResult status)
    {
        if (stripeEvent.Type == null)
        {
            return status;
        }

        switch (stripeEvent.Type)
        {
            case Events.PaymentIntentSucceeded:
                PaymentIntentSucceeded(stripeEvent);
                break;

            case Events.CustomerSubscriptionUpdated:
                PaymentUpdate(stripeEvent);
                break;

            case Events.CustomerSubscriptionDeleted:
                CustomerSubscriptionDeleted(stripeEvent);
                break;

            case Events.InvoicePaymentFailed:
                InvoicePaymentFailed(stripeEvent);
                break;

            case Events.PaymentMethodAttached:
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                Logg.r().Error(
                    $"The user paid with an incorrect payment method,  the CustomerId is {paymentMethod.CustomerId}.");
                break;
        }

        return new StatusCodeResult((int)HttpStatusCode.OK);
    }

    private void CustomerSubscriptionDeleted(Event stripeEvent)
    {
        var paymentDeleted = GetPaymentObjectAndUser<Subscription>(stripeEvent);
        var user = paymentDeleted.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has deleted the plan.";
            SetNewSubscriptionDate(user, paymentDeleted.paymentObject.CurrentPeriodEnd, log);
            Logg.SubscriptionLogger(StripePaymentEvents.Cancelled, user.Id);
        }

        LogErrorWhenUserNull(paymentDeleted.paymentObject.CustomerId, user);
    }

  
    public async Task<(Event stripeEvent, IActionResult httpResult)> GetEvent(HttpRequest request)
    {
        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();

        var endpointSecret = Settings.WebhookKeyStripe;

        try
        {
            var signatureHeader = request.Headers["Stripe-Signature"];
            var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

            return (stripeEvent, new StatusCodeResult((int)HttpStatusCode.OK));
        }
        catch (StripeException e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return (null, new StatusCodeResult((int)HttpStatusCode.BadRequest));
        }
        catch (Exception)
        {
            return (null, new StatusCodeResult((int)HttpStatusCode.InternalServerError));
        }
    }

    private (T paymentObject, User user) GetPaymentObjectAndUser<T>(Event stripeEvent)
        where T : class
    {
        var paymentObject = stripeEvent.Data.Object as T;

        if (paymentObject == null)
        {
            throw new ArgumentException("Invalid object type");
        }

        var customerId = ((dynamic)paymentObject).CustomerId;
        var user = _userReadingRepo.GetByStripeId(customerId);

        return (paymentObject, user);
    }

    private void InvoicePaymentFailed(Event stripeEvent)
    {
        var paymentFailed = GetPaymentObjectAndUser<Invoice>(stripeEvent);
        var user = paymentFailed.user;
        if (user != null)
        {
            var subscriptionDate = paymentFailed.paymentObject.BillingReason == "subscription_create"
                ? DateTime.Now
                : MaxValueMysql;

            var log = $"{user.Name} with userId: {user.Id}  has deleted the plan.";
            SetNewSubscriptionDate(user, subscriptionDate, log);
        }

        LogErrorWhenUserNull(paymentFailed.paymentObject.CustomerId, user);
    }

    private void LogErrorWhenUserNull(string customerId, User user)
    {
        if (user == null)
        {
            Logg.r().Error($"The user with the CustomerId:{customerId}  was not found");
        }
    }

    private void PaymentIntentSucceeded(Event stripeEvent)
    {
        var paymentIntent = GetPaymentObjectAndUser<PaymentIntent>(stripeEvent);
        var user = paymentIntent.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.";
            SetNewSubscriptionDate(user, MaxValueMysql, log, true);
        }

        LogErrorWhenUserNull(paymentIntent.paymentObject.CustomerId, user);
    }

    private void PaymentUpdate(Event stripeEvent)
    {
        var subscription = GetPaymentObjectAndUser<Subscription>(stripeEvent);
        var user = subscription.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.";
            DateTime endDate;
            if (subscription.paymentObject.CancelAtPeriodEnd)
            {
                endDate = subscription.paymentObject.CanceledAt ?? MaxValueMysql;
                Logg.SubscriptionLogger(StripePaymentEvents.Cancelled, user.Id);
            }
            else
            {
                endDate = MaxValueMysql;
                Logg.SubscriptionLogger(StripePaymentEvents.Success, user.Id);
            }

            SetNewSubscriptionDate(user, endDate, log, true);
        }

        LogErrorWhenUserNull(subscription.paymentObject.CustomerId, user);
    }

    private void SetNewSubscriptionDate(User user, DateTime date, string log, bool isIntentSucceeded = false)
    {
        if (user.SubscriptionStartDate == null && isIntentSucceeded)
        {
            user.SubscriptionStartDate = DateTime.Now;
        }

        user.EndDate = date;
        _userWritingRepo.Update(user);
        Logg.r().Information(log);
    }
}