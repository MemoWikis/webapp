using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class StripeAdminstrationController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly SubscriptionLogic _subscriptionLogic;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public StripeAdminstrationController(SessionUser sessionUser,
        SubscriptionLogic subscriptionLogic,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _subscriptionLogic = subscriptionLogic;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<JsonResult> CancelPlan()
    {
        return Json(await new BillingLogic(_httpContextAccessor,
            _webHostEnvironment)
            .DeletePlan(_sessionUser));
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