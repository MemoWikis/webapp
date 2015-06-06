using System;

public class MembershipModel : BaseModel
{
    public bool IsMember;
    public Membership Membership;
    public string BillingEmail { get; set; }
    public string BillingName { get; set; }
    public string BillingAddress { get; set; }
    public string PaymentPeriod { get; set; }
    public bool AutoRenewal { get; set; }

    public string SelectedPrice { get; set; }

    public string PriceLevel { get; set; }

    public MembershipModel()
    {
        if (!IsLoggedIn)
            return;

        var user = R<UserRepository>().GetById(UserId);

        IsMember = user.IsMember();
        Membership = user.CurrentMembership();

        BillingEmail = user.EmailAddress;
        BillingName = user.Name;
    }

    public Membership ToMembership()
    {
        if(PaymentPeriod != "fullYear" && PaymentPeriod != "halfYear")
            throw new Exception("invalid data");

        var membership = new Membership();
        membership.User = R<UserRepository>().GetById(UserId);

        membership.PeriodStart = DateTime.Now;
        membership.PeriodEnd = PaymentPeriod == "fullYear"
            ? DateTime.Now.AddYears(1).Date.AddSeconds(-1)
            : DateTime.Now.AddMonths(6).Date.AddSeconds(-1);

        membership.PricePerMonth = decimal.Parse(SelectedPrice);
        membership.PriceCategory = (PriceCategory) Enum.Parse(typeof (PriceCategory), PriceLevel);

        membership.AutoRenewal = AutoRenewal;

        membership.BillingEmail = BillingEmail;
        membership.BillingName = BillingName;
        membership.BillingAddress = BillingAddress;
       
        return membership;
    }
}