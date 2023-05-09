using System.Threading.Tasks;
using System.Web.Mvc;
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