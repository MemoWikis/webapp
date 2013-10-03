using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse
{
    public class SetMap : ClassMap<Set>
    {
        public SetMap()
        {
            Table("QuestionSet");
            Id(x => x.Id);
            Map(x => x.Name).Length(100);
            Map(x => x.Text).Length(Constants.VarCharMaxLength);

            Map(x => x.TotalRelevancePersonalAvg);
            Map(x => x.TotalRelevancePersonalEntries);

            HasMany(x => x.QuestionsInSet).Table("questionInSet").Cascade.None().OrderBy("Sort");
            HasManyToMany(x => x.Categories).Table("categories_to_sets").Cascade.SaveUpdate();
                
            References(x => x.Creator);

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
