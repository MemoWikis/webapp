using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VueApp;

public class StripeWebhookController(WebhookEventHandler _webhookEventHandler) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookEventHandler.Create(Request);
        return httpStatusCode;
    }
}