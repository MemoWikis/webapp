using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stripe;
using TrueOrFalse.Infrastructure.Logging;

public class Webhook
{
    private readonly DateTime MaxValueMysql = new(9999, 12, 31, 23, 59, 59);

    public async Task<HttpStatusCodeResult> Create(HttpContextBase context, HttpRequestBase baseRequest)
    {
        var eventAndStatus = await GetEvent(context, baseRequest);
        var stripeEvent = eventAndStatus.stripeEvent;
        var status = eventAndStatus.httpStatusCode;
        return Evaluate(stripeEvent, status);
    }

    public HttpStatusCodeResult Evaluate(Event stripeEvent, HttpStatusCodeResult status)
    {
        if (stripeEvent == null)
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

        return new HttpStatusCodeResult(HttpStatusCode.OK);
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

    private async Task<(Event stripeEvent, HttpStatusCodeResult httpStatusCode)> GetEvent(
        HttpContextBase context,
        HttpRequestBase baseRequest)
    {
        var json = await new StreamReader(context.Request.InputStream).ReadToEndAsync();
        var endpointSecret = Settings.WebhookKeyStripe;
        try
        {
            var signatureHeader = baseRequest.Headers["Stripe-Signature"];
            var stripeEvent = EventUtility.ConstructEvent(json,
                signatureHeader, endpointSecret);
            return (stripeEvent, new HttpStatusCodeResult(HttpStatusCode.OK));
        }
        catch (StripeException e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return (null, new HttpStatusCodeResult(HttpStatusCode.BadRequest));
        }
        catch (Exception e)
        {
            return (null, new HttpStatusCodeResult(HttpStatusCode.InternalServerError));
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
        var user = Sl.UserRepo.GetByStripeId(customerId);

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
        var subscription = GetPaymentObjectAndUser<Stripe.Subscription>(stripeEvent);
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

    private void SetNewSubscriptionDate(User user, DateTime date, string log, bool intentSucceeded = false)
    {
        if (user.SubscriptionStartDate == null && intentSucceeded)
        {
            user.SubscriptionStartDate = DateTime.Now;
        }

        user.EndDate = date;
        Sl.UserRepo.Update(user);
        Logg.r().Information(log);
    }
}