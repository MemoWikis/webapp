using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Stripe;

namespace VueApp;
public class PriceController: BaseController
{

    [AccessOnlyAsLoggedIn]
    public async Task<JsonResult> CreateCheckoutSession(string priceId)
    {
        var sessionId = await new SubscriptionLogic().CreateStripeSession(priceId); 
        if(sessionId.Equals("-1"))
        {
            return Json(new { success = false }); 
        }

        return Json(new { success = false, id = sessionId });
    }
}

public class CreateCheckoutSessionRequest
{
    public string PriceId { get; set; }
}
