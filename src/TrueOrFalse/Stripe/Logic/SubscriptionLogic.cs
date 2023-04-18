using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;

namespace TrueOrFalse.Stripe;

public class SubscriptionLogic
{
    public async Task<Session> InitiatePayment(string customerId,  string priceId )
    {
        var domain = "";
#if DEBUG
        domain = "http://memucho.local";
#else
        domain ="https://memucho.de";
#endif

        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                },
            },
            Mode = "subscription",
            SuccessUrl = domain + "/Price/Success?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "/Home/Cancel",
            Customer = customerId,
        };
        var service = new SessionService();
        var session = service.Create(options);
        return session;
    }

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
    private async Task<Customer> GetCustomer(string email)
    {
        var service = new CustomerService();
        var options = new CustomerListOptions
        {
            Email = email
        };
        var customers = await service.ListAsync(options);
        var customer = customers.FirstOrDefault();

        return customer;
    }

    public class SubscriptionCreateRequest
    {
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("items")]
        public List<SubscriptionItemOption> Items { get; set; }

        [JsonProperty("paymentBehavior")]
        public string PaymentBehavior { get; set; }
    }

    public class SubscriptionItemOption
    {
        [JsonProperty("price")]
        public string PriceId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }


}
