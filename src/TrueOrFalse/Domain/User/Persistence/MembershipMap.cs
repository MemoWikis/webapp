using FluentNHibernate.Mapping;

public class MembershipMap : ClassMap<Membership>
{
    public MembershipMap()
    {
        Id(x => x.Id);
        References(x => x.User);
        Map(x => x.BillingAddress);

        Map(x => x.PricePerMonth);
        Map(x => x.PriceCategory);

        Map(x => x.PaymentReceipt);
        Map(x => x.PaymentAmount);

        Map(x => x.PeriodStart);
        Map(x => x.PeriodEnd);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}