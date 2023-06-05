using System.Threading.Tasks;
using Stripe.BillingPortal;

namespace TrueOrFalse.Stripe.Logic;

public class BillingLogic : BaseStripeLogic
{
    public async Task<string> DeletePlan()
    {
        var stripeId = SessionUserLegacy.User.StripeId;
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