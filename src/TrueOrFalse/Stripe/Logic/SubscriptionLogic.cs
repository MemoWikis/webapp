using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using TrueOrFalse.Stripe.Logic;

namespace TrueOrFalse.Stripe;

public class SubscriptionLogic : BaseStripeLogic
{
    public async Task<string> CreateCustomer(string username, string email, int userId)
    {
        var optionsUser = new CustomerCreateOptions
        {
            Email = email,
            Name = username
        };
        var serviceUser = new CustomerService();
        var customer = await serviceUser.CreateAsync(optionsUser);

        var user = Sl.UserRepo.GetById(userId);
        user.StripeId = customer.Id;
        Sl.UserRepo.Update(user);

        return customer.Id;
    }

    public async Task<string> CreateStripeSession(string priceId)
    {
        var sessionUser = SessionUser.User;

        var customerId = "";
        if (sessionUser.StripeId == null)
        {
            customerId = await CreateCustomer(sessionUser.Name, sessionUser.EmailAddress, sessionUser.Id);
        }
        else
        {
            customerId = sessionUser.StripeId;
        }


        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
            SuccessUrl = CreateSiteLink("Preise"),
            CancelUrl = CreateSiteLink("cancel"),
            Customer = customerId
        };

        try
        {
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            return session.Id;
        }
        catch (StripeException e)
        {
            Logg.Error(e);
            return "-1";
        }
    }

    public class SubscriptionItemOption
    {
        [JsonProperty("price")]
        public string PriceId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}