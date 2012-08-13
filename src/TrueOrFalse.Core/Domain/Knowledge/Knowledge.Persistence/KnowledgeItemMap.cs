using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class KnowledgeItemMap : ClassMap<KnowledgeItem>
    {
        public KnowledgeItemMap()
        {
            Id(x => x.Id);
            Map(x => x.QuestionId);
            Map(x => x.UserId);
            Map(x => x.CorrectnessProbability);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
