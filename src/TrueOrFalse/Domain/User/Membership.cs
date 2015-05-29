using System;
using Seedworks.Lib.Persistence;

public class Membership : DomainEntity
{
    public virtual User User { get; set; }
    public virtual string BillingAddress { get; set; }
    public virtual decimal Price { get; set; }
    public virtual PriceCategory PriceCategory  { get; set; }
    public virtual DateTime PaymentReceipt { get; set; }
    public virtual decimal PaymentAmount { get; set; }
    public virtual DateTime PeriodStart { get; set; }
    public virtual DateTime PeriodEnd { get; set; }
}

public enum PriceCategory
{
    PriceReduced = 1,
    PriceNormal = 2,
    PriceSupporter = 3
}