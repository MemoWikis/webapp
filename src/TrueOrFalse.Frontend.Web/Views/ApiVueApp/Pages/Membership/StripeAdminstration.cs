using System.Threading.Tasks;
using System.Web.Mvc;
using Stripe.BillingPortal;
using TrueOrFalse.Stripe;

namespace VueApp;

public class StripeAdminstrationController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public ActionResult CancelPlan()
    {
        var stripeId = SessionUser.User.StripeId;
        var options = new SessionCreateOptions
        {
            Customer = stripeId,
            ReturnUrl = "https://example.com/account"
        };
        var service = new SessionService();
        var session = service.Create(options);

        return Json(session.Url, JsonRequestBehavior.AllowGet);
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