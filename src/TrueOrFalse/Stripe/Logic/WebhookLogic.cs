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

namespace TrueOrFalse.Stripe.Logic
{
    public class WebhookLogic
    {


        public async Task<HttpStatusCodeResult> Create(HttpContextBase context, HttpRequestBase baseRequest)
        {
            var json = await new StreamReader(context.Request.InputStream).ReadToEndAsync();
            var endpointSecret = Settings.WebhookKeyStripe;
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = baseRequest.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                    signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {

                    var paymentIntent = GetPaymentObjectAndUser<PaymentIntent>(stripeEvent);
                    var user = paymentIntent.user; 
                    if (user == null)
                    {
                        Logg.r().Error($"The user with the CustomerId:{paymentIntent.paymentObject.CustomerId}  was not found");
                    }
                    else
                    {
                        user.SubscriptionDuration = new DateTime(9999, 12, 31, 23, 59, 59);
                        Sl.UserRepo.Update(user);
                        Logg.r().Information(
                            $"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.");
                    }
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var paymentDeleted = GetPaymentObjectAndUser<Subscription>(stripeEvent);
                    var user = paymentDeleted.user;
                    user.SubscriptionDuration = paymentDeleted.paymentObject.CurrentPeriodEnd;
                    Sl.UserRepo.Update(user);
                    Logg.r().Information(
                        $"{user.Name} with userId: {user.Id}  has deleted the plan.");
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                }

                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public (T paymentObject, User user) GetPaymentObjectAndUser<T>(Event stripeEvent) where T : class
        {
            T paymentObject = stripeEvent.Data.Object as T;

            if (paymentObject == null)
            {
                throw new ArgumentException("Invalid object type");
            }

            var customerId = ((dynamic)paymentObject).CustomerId;
            var user = Sl.UserRepo.GetByStripeId(customerId);

            return (paymentObject,user);
        }
    }
}