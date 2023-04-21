using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;


namespace TrueOrFalse.Stripe;

public class SubscriptionLogic
{
    public async Task<string> CreateCustomer(string username, string email, int userId )
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

    public class SubscriptionItemOption
    {
        [JsonProperty("price")]
        public string PriceId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public async  Task<string> CreateStripeSession(string priceId)
    {
        var sessionUser = SessionUser.User;
        var abologik = new SubscriptionLogic();
        var customerId = "";
        if (sessionUser.StripeId == null)
        {
            customerId = await abologik.CreateCustomer(sessionUser.Name, sessionUser.EmailAddress, sessionUser.Id);
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
                new ()
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            SuccessUrl = "https://example.com/success",
            CancelUrl = "https://example.com/cancel",
            Customer = customerId,
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
        };
    }

}
