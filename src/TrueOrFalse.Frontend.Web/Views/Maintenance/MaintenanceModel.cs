using Stripe;
using System;
using TrueOrFalse.Web;

public class MaintenanceModel : BaseModel
{
    public UIMessage Message;

    public void AddAmount(
        string name,
        string description,
        int amount,
        string currency,
        string intervall)
    {
        StripeConfiguration.ApiKey = Settings.SecurityKeyStripe;

        var optionsProduct = new ProductCreateOptions
        {
            Name = "Monatlich",
            Description = "€3/Month",
        };
        var serviceProduct = new ProductService();
        Product product = serviceProduct.Create(optionsProduct);

        var optionsPrice = new PriceCreateOptions
        {
            UnitAmount = amount,
            Currency = currency,
            Recurring = new PriceRecurringOptions
            {
                Interval = intervall,
            },
            Product = product.Id
        };
        var servicePrice = new PriceService();
        Price price = servicePrice.Create(optionsPrice);
    }

}
