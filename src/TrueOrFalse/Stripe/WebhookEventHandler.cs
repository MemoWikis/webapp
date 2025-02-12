using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Net;
using TrueOrFalse.Infrastructure.Logging;

public class WebhookEventHandler : IRegisterAsInstancePerLifetime
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly DateTime MaxValueMysql = new(9999, 12, 31, 23, 59, 59);

    public WebhookEventHandler(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo)
    {
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
    }

    public async Task<IActionResult> Create(HttpRequest request)
    {
        var eventAndStatus = await GetEvent(request);
        var stripeEvent = eventAndStatus.stripeEvent;

        return Evaluate(stripeEvent, eventAndStatus.httpResult);
    }

    public IActionResult Evaluate(Event stripeEvent, IActionResult status)
    {
        if (stripeEvent == null || stripeEvent.Type == null)
        {
            return status;
        }

        Logg.r.Information($"StripeEvent: {stripeEvent.Type}, {stripeEvent.Data.Object}, liveMode: {stripeEvent.Livemode}");

        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                PaymentIntentSucceeded(stripeEvent);
                break;

            case "customer.subscription.updated":
                PaymentUpdate(stripeEvent);
                break;

            case "customer.subscription.deleted":
                CustomerSubscriptionDeleted(stripeEvent);
                break;

            case "invoice.payment_failed":
                InvoicePaymentFailed(stripeEvent);
                break;

            case "payment_method.attached":
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                Logg.r.Error($"The user paid with an incorrect payment method, the CustomerId is {paymentMethod.CustomerId}.");
                break;

            default:
                Logg.r.Warning($"Unhandled Stripe event type: {stripeEvent.Type}");
                break;
        }

        return new StatusCodeResult((int)HttpStatusCode.OK);
    }

    public async Task<(Event stripeEvent, IActionResult httpResult)> GetEvent(HttpRequest request)
    {
        // Enable buffering so the request body can be read multiple times
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var json = await reader.ReadToEndAsync();
        request.Body.Position = 0; // Rewind the stream for the next reader

        var endpointSecret = Settings.WebhookKeyStripe;

        try
        {
            var signatureHeader = request.Headers["Stripe-Signature"];
            var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

            return (stripeEvent, new StatusCodeResult((int)HttpStatusCode.OK));
        }
        catch (StripeException e)
        {
            Logg.r.Error($"Stripe - StripeException: {e.Message}", e);

            return (null, new StatusCodeResult((int)HttpStatusCode.BadRequest));
        }
        catch (Exception ex)
        {
            Logg.r.Error($"Stripe - Exception: {ex.Message}", ex);
            return (null, new StatusCodeResult((int)HttpStatusCode.InternalServerError));
        }
    }

    private void CustomerSubscriptionDeleted(Event stripeEvent)
    {
        var paymentDeleted = GetPaymentObjectAndUser<Stripe.Subscription>(stripeEvent);
        var user = paymentDeleted.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has deleted the plan.";
            SetNewSubscriptionDate(user, paymentDeleted.paymentObject.CurrentPeriodEnd, log);
            Logg.SubscriptionLogger(StripePaymentEvents.Cancelled, user.Id);
        }

        LogErrorWhenUserNull(paymentDeleted.paymentObject.CustomerId, user);
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
            var currentSubscriptionDate = user.EndDate;
            var newSubscriptionDate = DateTime.Now;

            var log = $"Invoice payment for user with userId: {user.Id} failed. BillingReason: {paymentFailed.paymentObject.BillingReason}. Old SubscriptionEndDate was: {currentSubscriptionDate}, New SubscriptionEndDate is {newSubscriptionDate}.";
            SetNewSubscriptionDate(user, newSubscriptionDate, log);
        }

        LogErrorWhenUserNull(paymentFailed.paymentObject.CustomerId, user);
    }

    private void LogErrorWhenUserNull(string customerId, User user)
    {
        if (user == null)
        {
            Logg.r.Error($"The user with the CustomerId:{customerId}  was not found");
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
        var subscription = GetPaymentObjectAndUser<Stripe.Subscription>(stripeEvent);
        var user = subscription.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.";
            DateTime endDate;
            if (subscription.paymentObject.CancelAtPeriodEnd)
            {
                endDate = subscription.paymentObject.CurrentPeriodEnd;
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

    private void SetNewSubscriptionDate(User user, DateTime date, string log, bool intentSucceeded = false)
    {
        if (user.SubscriptionStartDate == null && intentSucceeded)
        {
            user.SubscriptionStartDate = DateTime.Now;
        }

        user.EndDate = date;
        _userWritingRepo.Update(user);
        Logg.r.Information(log);
    }
}