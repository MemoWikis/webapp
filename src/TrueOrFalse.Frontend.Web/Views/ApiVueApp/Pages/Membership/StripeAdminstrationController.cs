using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

    public readonly record struct CompletedSubscriptionJson(string priceId);

    public readonly record struct SubscriptionResult(bool Success, string Id);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public async Task<SubscriptionResult> CompletedSubscription(
        [FromBody] CompletedSubscriptionJson json)
    {
        var sessionId =
            await _stripeSubscriptionManger.CreateStripeSubscriptionSession(json.priceId);
        if (sessionId.Equals("-1"))
        {
            return new SubscriptionResult { Success = false };
        }

        return new SubscriptionResult { Success = true, Id = sessionId };
    }
}