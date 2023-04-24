using Stripe;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Stripe.Logic;

public class WebhookLogic
{

    private DateTime MaxValueMysql = new DateTime(9999, 12, 31, 23, 59, 59); 
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

    private (T paymentObject, User user) GetPaymentObjectAndUser<T>(Event stripeEvent) where T : class
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

    private async Task<(Event stripeEvent, HttpStatusCodeResult httpStatusCode)> GetEvent(HttpContextBase context, HttpRequestBase baseRequest)
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

    private void PaymentIntentSucceeded(Event stripeEvent)
    {
        var paymentIntent = GetPaymentObjectAndUser<PaymentIntent>(stripeEvent);
        var user = paymentIntent.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.";
            SetNewSubscriptionDate(user, MaxValueMysql, log);
        }
        LogErrorWhenUserNull(paymentIntent.paymentObject.CustomerId, user);
    }

    private void CustomerSubscriptionDeleted(Event stripeEvent)
    {
        var paymentDeleted = GetPaymentObjectAndUser<Subscription>(stripeEvent);
        var user = paymentDeleted.user;
        if (user != null)
        {
            var log = $"{user.Name} with userId: {user.Id}  has deleted the plan.";
            SetNewSubscriptionDate(user, paymentDeleted.paymentObject.CurrentPeriodEnd, log);
        }
        LogErrorWhenUserNull(paymentDeleted.paymentObject.CustomerId, user);
    }

    private void InvoicePaymentFailed(Event stripeEvent)
    {
        var paymentFailed = GetPaymentObjectAndUser<Invoice>(stripeEvent);
        var user = paymentFailed.user;
        if (user != null)
        {
            var subscriptionDate = paymentFailed.paymentObject.BillingReason == "subscription_create" ? DateTime.Now : MaxValueMysql; 
          
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

    private void SetNewSubscriptionDate(User user, DateTime date, string log)
    {
            user.SubscriptionDuration = date;
            Sl.UserRepo.Update(user);
            Logg.r().Information(log);
    }

    public void FetchFailedWebhookRequests()
    {
        var requestOptions = new RequestOptions();
        requestOptions.ApiKey = StripeConfiguration.ApiKey;

        var options = new WebhookEndpointListOptions
        {
            Limit = 100,
        };

        var service = new WebhookEndpointService();
        StripeList<WebhookEndpoint> webhookEndpoints = service.List(options, requestOptions);

        foreach (var webhookEndpoint in webhookEndpoints)
        {

            if (webhookEndpoint.Id.Equals("we_1MxpEsCAfoBJxQho8deug8KF"))
            {
                Console.WriteLine(webhookEndpoint);

                var eventOptions = new EventListOptions
                {
                    DeliverySuccess = false, // Nur fehlgeschlagene Requests
                    Limit = 100,
                };

                var eventService = new EventService();
                StripeList<Event> events = eventService.List(eventOptions, requestOptions);

                foreach (var eventItem in events)
                {
                    Evaluate(eventItem, new HttpStatusCodeResult(HttpStatusCode.OK));

                }
            }
        }
    }
}
