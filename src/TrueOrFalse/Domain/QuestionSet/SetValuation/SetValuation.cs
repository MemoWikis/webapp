using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("SetId={SetId} IsInWuwi: {IsInWishKnowledge()}")]
public class SetValuation : DomainEntity
{
    public virtual int UserId { get; set; }
    public virtual int SetId { get; set; }

    public virtual int RelevancePersonal { get; set; }

    public virtual int CountNotLearned { get; set; }
    public virtual int CountNeedsLearning { get; set; }
    public virtual int CountNeedsConsolidation { get; set; }
    public virtual int CountSolid { get; set; }

    public SetValuation()
    {
        RelevancePersonal = -1;
    }

    public virtual bool IsInWishKnowledge() { return RelevancePersonal > 0; }

    public virtual void UpdateKnowledgeSummary()
    {
        var knowledgeSummary = KnowledgeSummaryLoader.Run(UserId, SetId, false);
        CountNotLearned = knowledgeSummary.NotLearned;
        CountNeedsLearning = knowledgeSummary.NeedsLearning;
        CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        CountSolid = knowledgeSummary.Solid;
    }
}