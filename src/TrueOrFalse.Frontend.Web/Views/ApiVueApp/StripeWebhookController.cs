using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class StripeWebhookController : Controller
{
    public async Task<ActionResult> Webhook()
    {
        var httpStatusCode = await new Webhook().Create(HttpContext, Request);
        return httpStatusCode;
    }
}