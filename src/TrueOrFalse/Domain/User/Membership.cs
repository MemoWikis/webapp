using System;
using Seedworks.Lib.Persistence;

[Serializable]
public class Membership : DomainEntity
{
    public virtual User User { get; set; }
    public virtual string BillingEmail { get; set; }
    public virtual string BillingName { get; set; }
    public virtual string BillingAddress { get; set; }
    public virtual decimal PricePerMonth { get; set; }
    public virtual PriceCategory PriceCategory  { get; set; }
    public virtual DateTime PaymentReceipt { get; set; }
    public virtual decimal PaymentAmount { get; set; }
    public virtual DateTime PeriodStart { get; set; }
    public virtual DateTime PeriodEnd { get; set; }
    public virtual bool AutoRenewal { get; set; }

    public virtual bool IsActive()
    {
        return DateTime.Now >= PeriodStart && DateTime.Now <= PeriodEnd;
    }
    public virtual bool IsActive(DateTime givenDateTime)
    {
        return givenDateTime >= PeriodStart.AddMinutes(-1) && givenDateTime <= PeriodEnd;
    }
}

public enum PriceCategory
{
    Reduced = 1,
    Normal = 2,
    Supporter = 3
}