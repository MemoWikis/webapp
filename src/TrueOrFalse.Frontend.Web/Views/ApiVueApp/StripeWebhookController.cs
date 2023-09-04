using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class StripeWebhookController : Controller
{
    private readonly WebhookLogic _webhookLogic;

    public StripeWebhookController(WebhookLogic webhookLogic)
    {
        _webhookLogic = webhookLogic;
    }
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookEventHandler.Create();
        return httpStatusCode;
    }
}