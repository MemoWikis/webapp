[ApiController]
public class StripeWebhookController(WebhookEventHandler _webhookEventHandler) : ApiBaseController
{
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookEventHandler.Create(Request);
        return httpStatusCode;
    }
}