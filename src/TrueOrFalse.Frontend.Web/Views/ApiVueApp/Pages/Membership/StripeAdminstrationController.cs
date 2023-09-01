﻿using System.Threading.Tasks;
using System.Web.Mvc;

namespace VueApp;

public class StripeAdminstrationController : Controller
{
    private readonly SessionUser _sessionUser;

    public StripeAdminstrationController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<JsonResult> CancelPlan()
    {
        return Json(await StripeSubscriptionHelper.GetCancelPlanSessionUrl(_sessionUser), JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> CompletedSubscription(string priceId)
    {
        var sessionId = await new StripeSubscriptionHelper(_sessionUser).CreateStripeSubscriptionSession(priceId);
        if (sessionId.Equals("-1"))
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, id = sessionId });
    }
}