using System.Threading.Tasks;
using Stripe.BillingPortal;


public class BillingLogic : BaseStripeLogic
{
    public async Task<string> DeletePlan(SessionUser sessionUser)
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