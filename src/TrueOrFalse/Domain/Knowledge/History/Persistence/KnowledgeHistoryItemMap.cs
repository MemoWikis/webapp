using FluentNHibernate.Mapping;

public class KnowledgeHistoryItemMap : ClassMap<KnowledgeHistoryItem>
{
    public KnowledgeHistoryItemMap()
    {
        Id(x => x.Id);
        Map(x => x.AmountActiveKnowledge);
        Map(x => x.AmountInactiveKnowledge);
        Map(x => x.AmountNoDataKnowledge);
        Map(x => x.DateCreated);            
    }
}