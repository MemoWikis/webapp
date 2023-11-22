using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class StripeWebhookController : Controller
{
    private readonly WebhookEventHandler _webhookEventHandler;

    public StripeWebhookController(WebhookEventHandler webhookLogic)
    {
        _webhookEventHandler = webhookLogic;
    }
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookEventHandler.Create();
        return httpStatusCode;
    }
}