using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Stripe;
using Stripe.Checkout;
using TrueOrFalse.Stripe.Logic;

namespace VueApp;
public class PriceController: BaseController
{

    [AccessOnlyAsLoggedIn]
    public async Task<JsonResult> CreateCheckoutSession(string priceId)
    {
        var sessionUser = SessionUser.User;
        var abologik = new SubscriptionLogic();
        var customerId = "";
        if (sessionUser.StripeId == null)
        {
            customerId = await abologik.CreateCustomer(sessionUser.Name, sessionUser.EmailAddress, sessionUser.Id);
        }
        else
        {
            customerId = sessionUser.StripeId;
        }


        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new ()
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            SuccessUrl = "https://example.com/success",
            CancelUrl = "https://example.com/cancel",
            Customer = customerId,
        };

        try
        {
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            return Json(new { success = true,id = session.Id }); ;
        }
        catch (StripeException e)
        {
            return Json(new
            {
                success = false
            });
        };
    }
}

public class CreateCheckoutSessionRequest
{
    public string PriceId { get; set; }
}
