using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Stripe;
using TrueOrFalse.Stripe.Logic;

namespace VueApp;

public class StripeWebhookController : Controller
{
    public async Task<ActionResult> Webhook()
    {
        var httpStatusCode = await new WebhookLogic().Create(HttpContext, Request);
        return httpStatusCode; 
    }
}