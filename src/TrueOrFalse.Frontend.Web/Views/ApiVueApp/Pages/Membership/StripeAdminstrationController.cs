using System.Threading.Tasks;
using HelperClassesControllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class StripeAdminstrationController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly StripeSubscriptionHelper _stripeSubscriptionHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public StripeAdminstrationController(SessionUser sessionUser,
        StripeSubscriptionHelper stripeSubscriptionHelper,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _stripeSubscriptionHelper = stripeSubscriptionHelper;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<JsonResult> CancelPlan()
    {
        return Json(await _stripeSubscriptionHelper.GetCancelPlanSessionUrl());
    }

    public readonly record struct CompletedSubscriptionJson(string priceId);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<JsonResult> CompletedSubscription([FromBody] CompletedSubscriptionJson json)
    {
        var sessionId = await _stripeSubscriptionHelper.CreateStripeSubscriptionSession(json.priceId);
        if (sessionId.Equals("-1"))
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, id = sessionId });
    }
}