using System.Threading.Tasks;
using Stripe.BillingPortal;


public class CancelPlan : StripeBase
{
    public async Task<string> GetCancelPlanSessionUrl(SessionUser sessionUser)
    {
        var stripeId = sessionUser.User.StripeId;
        var options = new SessionCreateOptions
        {
            Customer = stripeId,
            ReturnUrl = CreateSiteLink("")
        };
        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}