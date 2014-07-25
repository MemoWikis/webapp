using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
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
}
