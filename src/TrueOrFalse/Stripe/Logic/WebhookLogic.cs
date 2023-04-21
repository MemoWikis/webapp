using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace TrueOrFalse.Stripe.Logic;

public class WebhookLogic
{
    public async Task<HttpStatusCodeResult> Create(HttpContextBase context, HttpRequestBase baseRequest)
    {
        var eventAndStatus = await GetEvent(context, baseRequest);
        var stripeEvent = eventAndStatus.stripeEvent;
        var status = eventAndStatus.httpStatusCode;

        if (stripeEvent == null)
            return status;

        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        {
            PaymentIntentSucceeded(stripeEvent);
        }
        else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
        {
            CustomerSubscriptionDeleted(stripeEvent);
        }
        else if (stripeEvent.Type == Events.InvoicePaymentFailed)
        {
            InvoicePaymentFailed(stripeEvent);
        }
        else if (stripeEvent.Type == Events.PaymentMethodAttached)
        {
            var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
            Logg.r().Error($"The user paid with an incorrect payment method,  the CustomerId is {paymentMethod.CustomerId}.");
        }

        return new HttpStatusCodeResult(HttpStatusCode.OK);
    }

    public (T paymentObject, User user) GetPaymentObjectAndUser<T>(Event stripeEvent) where T : class
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

    public async Task<(Event stripeEvent, HttpStatusCodeResult httpStatusCode)> GetEvent(HttpContextBase context, HttpRequestBase baseRequest)
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

    public void PaymentIntentSucceeded(Event stripeEvent)
    {
        var paymentIntent = GetPaymentObjectAndUser<PaymentIntent>(stripeEvent);
        var user = paymentIntent.user;
        LogErrorWhenUserNull(paymentIntent.paymentObject.CustomerId, user);

        var log = $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.";
        SetNewSubscriptionDate(user, new DateTime(9999, 12, 31, 23, 59, 59), log);
    }

    public void CustomerSubscriptionDeleted(Event stripeEvent)
    {
        var paymentDeleted = GetPaymentObjectAndUser<Subscription>(stripeEvent);
        var user = paymentDeleted.user;
        LogErrorWhenUserNull(paymentDeleted.paymentObject.CustomerId, user);

        var log = $"{user.Name} with userId: {user.Id}  has deleted the plan.";
        SetNewSubscriptionDate(user, paymentDeleted.paymentObject.CurrentPeriodEnd, log);
    }

    public void InvoicePaymentFailed(Event stripeEvent)
    {
        var paymentFailed = GetPaymentObjectAndUser<Invoice>(stripeEvent);
        var user = paymentFailed.user;
        LogErrorWhenUserNull(paymentFailed.paymentObject.CustomerId, user);

        var log = $"{user.Name} with userId: {user.Id}  has deleted the plan."; 
        SetNewSubscriptionDate(user, DateTime.Now, log);
    }

    public void LogErrorWhenUserNull(string customerId, User user) 
    {
        if (user == null)
        {
            Logg.r().Error($"The user with the CustomerId:{customerId}  was not found");
        }
    }

    public void SetNewSubscriptionDate(User user, DateTime date, string log)
    {
        if (user != null)
        {
            user.SubscriptionDuration = date;
            Sl.UserRepo.Update(user);
            Logg.r().Information(log);
        }
    }
}
