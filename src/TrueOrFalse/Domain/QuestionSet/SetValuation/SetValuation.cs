using Seedworks.Lib.Persistence;

public class SetValuation : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int SetId { get; set; }

    public virtual int RelevancePersonal { get; set; }

    public SetValuation()
    {
        RelevancePersonal = -1;
    }

    public virtual bool IsInWishknowledge(){ return RelevancePersonal != -1; }
}