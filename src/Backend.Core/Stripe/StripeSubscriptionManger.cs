﻿using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

public class StripeSubscriptionManger(
    SessionUser sessionUser,
    UserReadingRepo userReadingRepo,
    UserWritingRepo userWritingRepo)
    : IRegisterAsInstancePerLifetime
{
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

        var user = userReadingRepo.GetById(userId);
        user.StripeId = customer.Id;
        userWritingRepo.Update(user);

        return customer.Id;
    }

    public async Task<string> CreateStripeSubscriptionSession(string priceId)
    {
        var user = sessionUser.User;

        string customerId;
        if (user.StripeId == null)
        {
            customerId = await CreateStripeCustomer(
                user.Name,
                user.EmailAddress,
                user.Id);
        }
        else
        {
            customerId = user.StripeId;
        }

        var stripeReturnUrlGenerator = new StripeReturnUrlGenerator();
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card",
                //"paypal"
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
            ErrorLogging.Log(e);
            return "-1";
        }
    }

    public async Task<string> GetCancelPlanSessionUrl()
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
        [JsonProperty("price")] public string PriceId { get; set; }

        [JsonProperty("quantity")] public int Quantity { get; set; }
    }
}