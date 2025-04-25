using System.Threading.Tasks;



public class StripeWebhookController(WebhookEventHandler _webhookEventHandler) : ApiBaseController
{
    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookEventHandler.Create(Request);
        return httpStatusCode;
    }
}