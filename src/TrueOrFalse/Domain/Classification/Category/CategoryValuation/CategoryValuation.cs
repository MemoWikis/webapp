using Seedworks.Lib.Persistence;

public class CategoryValuation : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int CategoryId { get; set; }

    public virtual int RelevancePersonal { get; set; }

    public CategoryValuation()
    {
        RelevancePersonal = -1;
    }

    public virtual bool IsInWishKnowledge() { return RelevancePersonal > 0; }
}
