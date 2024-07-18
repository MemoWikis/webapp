using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VueApp;

public class StripeAdminstrationController(StripeSubscriptionManger _stripeSubscriptionManger)
    : Controller
{
    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public async Task<string> CancelPlan()
    {
        return await _stripeSubscriptionManger.GetCancelPlanSessionUrl();
    }

    public readonly record struct CompletedSubscriptionRequest(string PriceId);

    public readonly record struct CompletedSubscriptionResult(bool Success, string Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<CompletedSubscriptionResult> CompletedSubscription([FromBody] CompletedSubscriptionRequest request)
    {
        var sessionId = await _stripeSubscriptionManger.CreateStripeSubscriptionSession(request.PriceId);
        if (sessionId == null)
        {
            return new CompletedSubscriptionResult { Success = false };
        }

        return new CompletedSubscriptionResult { Success = true, Id = sessionId };
    }
}