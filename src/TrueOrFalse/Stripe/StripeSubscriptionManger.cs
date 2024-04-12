using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

public class StripeSubscriptionManger : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public StripeSubscriptionManger(
        SessionUser sessionUser,
        UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
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

        var user = _userReadingRepo.GetById(userId);
        user.StripeId = customer.Id;
        _userWritingRepo.Update(user);

        return customer.Id;
    }

    public async Task<string> CreateStripeSubscriptionSession(string priceId)
    {
        var sessionUser = _sessionUser.User;

        string customerId;
        if (sessionUser.StripeId == null)
        {
            customerId = await CreateStripeCustomer(sessionUser.Name, sessionUser.EmailAddress,
                sessionUser.Id);
        }
        else
        {
            customerId = sessionUser.StripeId;
        }

        var stripeReturnUrlGenerator =
            new StripeReturnUrlGenerator(_httpContextAccessor, _webHostEnvironment);
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card",
                "paypal"
                //, "sofort"
            },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
            SuccessUrl = stripeReturnUrlGenerator.Create("Preise"),
            CancelUrl = stripeReturnUrlGenerator.Create("Preise"),
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

    public async Task<string> GetCancelPlanSessionUrl()
    {
        var stripeId = _sessionUser.User.StripeId;
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = stripeId,
            ReturnUrl = new StripeReturnUrlGenerator(_httpContextAccessor, _webHostEnvironment)
                .Create("")
        };
        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public class SubscriptionItemOption
    {
        [JsonProperty("price")] public string PriceId { get; set; }

        [JsonProperty("quantity")] public int Quantity { get; set; }
    }
}