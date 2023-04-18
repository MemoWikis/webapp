using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Stripe;

namespace VueApp;

public class StripeWebhookController : Controller
{
    public async Task<ActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.InputStream).ReadToEndAsync();
        var endpointSecret = Settings.WebhookKeyStripe;
        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            stripeEvent = EventUtility.ConstructEvent(json,
                signatureHeader, endpointSecret);

            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                var customerId = paymentIntent.CustomerId; 
                var user = Sl.UserRepo.GetByStripeId(customerId);
                if (user == null)
                {
                    Logg.r().Error($"The user with the CustomerId:{customerId}  was not found");
                }
                else
                {
                    user.SubscriptionDuration = DateTime.MaxValue;
                    Sl.UserRepo.Update(user);
                    Logg.r().Information($"{user.Name} with userId: {user.Id}  has successfully subscribed to a plan.");
                }

                
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
}