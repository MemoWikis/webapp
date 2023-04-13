using Newtonsoft.Json;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Session = Stripe.Checkout.Session;

namespace Stripe.ControllerLogiken;

public class AboControllerLogik
{
    public async Task<Session> Init(string email, string username, string priceId )
    {
        var domain = "http://localhost:5069";
        var customer = await GetCustomer(email);

        if (customer == null)
        {
            var optionsUser = new CustomerCreateOptions
            {
                Email = email,
                Name = username
            };
            var serviceUser = new CustomerService();
            customer = await serviceUser.CreateAsync(optionsUser);
        };

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
            SuccessUrl = domain + "/Home/Success?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "/Home/Cancel",
            Customer = customer.Id,
        };
        var service = new SessionService();
        var session = service.Create(options);
        return session;
    }

    private async Task<Customer?> GetCustomer(string email)
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
