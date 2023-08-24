using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class StripeWebhookController : Controller
{
    private readonly WebhookLogic _webhookLogic;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StripeWebhookController(WebhookLogic webhookLogic,
        IHttpContextAccessor httpContextAccessor)
    {
        _webhookLogic = webhookLogic;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Webhook()
    {
        var httpStatusCode = await _webhookLogic.Create();
        return httpStatusCode;
    }
}