
using System;
using NUnit.Framework;

public class Membership_persistence : BaseTest
{
    [Test]
    public void Should_persist()
    {
        var context = ContextUser.New().Add("Firstname Lastname").Persist();

        var membership = new Membership
        {
            User = context.All[0],
            BillingAddress = "Billing address",
            Price = 2.50m,
            PriceCategory = PriceCategory.PriceNormal,
            PaymentAmount = 2.50m,
            PeriodStart = DateTime.Now,
            PeriodEnd = DateTime.Now.AddMonths(6)
        };

        R<MembershipRepo>().Create(membership);

        RecycleContainer();

        var membershipFromDb = R<MembershipRepo>().GetById(membership.Id);
        
        Assert.That(membershipFromDb.Price, Is.EqualTo(2.50m));
        Assert.That(membershipFromDb.User.Name, Is.EqualTo("Firstname Lastname"));
    }
}