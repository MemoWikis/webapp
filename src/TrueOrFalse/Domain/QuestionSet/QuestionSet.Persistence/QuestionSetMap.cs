using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse
{
    public class QuestionSetMap : ClassMap<QuestionSet>
    {
        public QuestionSetMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Length(100);
            Map(x => x.Text).Length(Constants.VarCharMaxLength);

            HasMany(x => x.QuestionsInSet).Table("questionInSet").Cascade.None().OrderBy("Sort");
                
            References(x => x.Creator);

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
