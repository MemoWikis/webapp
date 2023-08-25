using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Stripe.BillingPortal;


public class BillingLogic : BaseStripeLogic
{
    public BillingLogic(IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webEnbEnvironment) 
        : base(httpContextAccessor, webEnbEnvironment)
    {
        
    }
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