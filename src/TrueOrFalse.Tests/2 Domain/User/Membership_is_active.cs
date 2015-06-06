using System;
using NUnit.Framework;

class Membership_is_active : BaseTest
{
    [Test]
    public void Should_tell_if_period_is_active()
    {
        var dateTimeNow = DateTime.Now;
        var dateTimeEnd = DateTime.Now.AddMonths(6);
        var membership = new Membership { PeriodStart = dateTimeNow, PeriodEnd = dateTimeEnd };

        Assert.That(membership.IsActive(dateTimeNow));
        Assert.That(membership.IsActive(dateTimeEnd));
    }
}

