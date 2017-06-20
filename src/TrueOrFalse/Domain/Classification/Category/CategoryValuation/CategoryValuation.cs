using Seedworks.Lib.Persistence;

public class CategoryValuation : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int CategoryId { get; set; }

    public virtual int RelevancePersonal { get; set; }

    public virtual int CountNotLearned { get; set; }
    public virtual int CountNeedsLearning { get; set; }
    public virtual int CountNeedsConsolidation { get; set; }
    public virtual int CountSolid { get; set; }

    public CategoryValuation()
    {
        RelevancePersonal = -1;
    }

    public virtual bool IsInWishKnowledge() { return RelevancePersonal > 0; }
}
