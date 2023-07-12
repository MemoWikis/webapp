using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class StripeWebhookController : Controller
{
    private readonly WebhookLogic _webhookLogic;

    public StripeWebhookController(WebhookLogic webhookLogic)
    {
        _webhookLogic = webhookLogic;
    }
    public async Task<ActionResult> Webhook()
    {
        var httpStatusCode = await _webhookLogic.Create(HttpContext, Request);
        return httpStatusCode;
    }
}