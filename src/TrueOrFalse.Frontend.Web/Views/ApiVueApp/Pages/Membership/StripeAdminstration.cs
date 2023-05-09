using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Stripe;
using TrueOrFalse.Stripe.Logic;

namespace VueApp;

public class StripeAdminstrationController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<JsonResult> CancelPlan()
    {
        return Json(await new BillingLogic().DeletePlan(), JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> CompletedSubscription(string priceId)
    {
        var sessionId = await new SubscriptionLogic().CreateStripeSession(priceId);
        if (sessionId.Equals("-1"))
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, id = sessionId });
    }
}