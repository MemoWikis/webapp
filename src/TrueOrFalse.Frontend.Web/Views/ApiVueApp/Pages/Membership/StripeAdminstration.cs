using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class StripeAdminstrationController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly SubscriptionLogic _subscriptionLogic;

    public StripeAdminstrationController(SessionUser sessionUser, SubscriptionLogic subscriptionLogic)
    {
        _sessionUser = sessionUser;
        _subscriptionLogic = subscriptionLogic;
    }
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<JsonResult> CancelPlan()
    {
        return Json(await new BillingLogic().DeletePlan(_sessionUser), JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> CompletedSubscription(string priceId)
    {
        var sessionId = await _subscriptionLogic.CreateStripeSession(priceId);
        if (sessionId.Equals("-1"))
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, id = sessionId });
    }
}