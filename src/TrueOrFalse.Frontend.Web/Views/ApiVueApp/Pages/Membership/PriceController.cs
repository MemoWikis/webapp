using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Stripe;
using Stripe.Checkout;
using TrueOrFalse.Stripe.Logik;

namespace VueApp;
public class PriceController: BaseController
{
    
   
    public async Task<JsonResult> MonthlySubscription(string priceId)
    {
        var sessionUser = SessionUser.User;
        var customerId = "";
        var abologik = new AboLogik(); 
        if (sessionUser.StripeId == null)
        {
            customerId = await abologik.CreateCustomer(sessionUser.Name, sessionUser.EmailAddress, sessionUser.Id); 
        }
        else
        {
            customerId = sessionUser.StripeId;
        }

        var session = await abologik.InitiatePayment(customerId, priceId);
        return Json(session.Id, JsonRequestBehavior.AllowGet); 
    }

    [AccessOnlyAsLoggedIn]
    public async Task<ActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new ()
                {
                    Price = request.PriceId,
                    Quantity = 1,
                },
            },
            SuccessUrl = "https://example.com/success",
            CancelUrl = "https://example.com/cancel",
            Customer = request.CustomerId
        };

        try
        {
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            return Json(new { id = session.Id }); ;
        }
        catch (StripeException e)
        {
            return Json("fail");
        };
    }
}

public class CreateCheckoutSessionRequest
{
    public string CustomerId { get; set; }
    public string PriceId { get; set; }
}
