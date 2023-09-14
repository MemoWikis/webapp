using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

public class StripeSubscriptionHelper
{
    private readonly SessionUser _sessionUser;

    public StripeSubscriptionHelper(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    private async Task<string> CreateStripeCustomer(string username, string email, int userId)
    {
        var optionsUser = new CustomerCreateOptions
        {
            Email = email,
            Name = username,
            Address = new AddressOptions
            {
                //So far, we only allow for subscriptions from Germany and set the location for the customer ourselves:
                Country = "DE",
            }
        };
        var serviceUser = new CustomerService();
        var customer = await serviceUser.CreateAsync(optionsUser);

        var user = Sl.UserRepo.GetById(userId);
        user.StripeId = customer.Id;
        Sl.UserRepo.Update(user);

        return customer.Id;
    }

    public async Task<string> CreateStripeSubscriptionSession(string priceId)
    {
        var sessionUser = _sessionUser.User;

        string customerId;
        if (sessionUser.StripeId == null)
        {
            customerId = await CreateStripeCustomer(sessionUser.Name, sessionUser.EmailAddress, sessionUser.Id);
        }
        else
        {
            customerId = sessionUser.StripeId;
        }

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card", "paypal", "sofort" },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
            SuccessUrl = StripeReturnUrlGenerator.Create("Preise"),
            CancelUrl = StripeReturnUrlGenerator.Create("Preise"),
            Customer = customerId,
            AutomaticTax = new SessionAutomaticTaxOptions
            {
                Enabled = true
            }
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

    public static async Task<string> GetCancelPlanSessionUrl(SessionUser sessionUser)
    {
        var stripeId = sessionUser.User.StripeId;
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = stripeId,
            ReturnUrl = StripeReturnUrlGenerator.Create("")
        };
        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public class SubscriptionItemOption
    {
        [JsonProperty("price")]
        public string PriceId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}