using System.Diagnostics;

[DebuggerDisplay("PageId={PageId} IsInWishknowledge: {IsInWishknowledge()}")]
public class PageValuation : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int PageId { get; set; }

    public virtual int RelevancePersonal { get; set; }

    public virtual int CountNotLearned { get; set; }
    public virtual int CountNeedsLearning { get; set; }
    public virtual int CountNeedsConsolidation { get; set; }
    public virtual int CountSolid { get; set; }

    public PageValuation()
    {
        RelevancePersonal = -1;
    }

    public virtual bool IsInWishknowledge() { return RelevancePersonal > 0; }
}
