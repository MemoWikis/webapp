using Seedworks.Lib.Persistence;

public class KnowledgeHistoryItem : DomainEntity
{
    public virtual decimal AmountActiveKnowledge { get; set; }
    public virtual decimal AmountInactiveKnowledge { get; set; }
    public virtual decimal AmountNoDataKnowledge { get; set; }
}