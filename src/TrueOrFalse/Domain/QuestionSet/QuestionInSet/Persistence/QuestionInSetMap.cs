using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
    public class QuestionInSetMap : ClassMap<QuestionInSet>
    {
        public QuestionInSetMap()
        {
            Table("questionInSet");
            Id(x => x.Id);
            
            References(x => x.Question).Cascade.None();
            References(x => x.Set).Cascade.None();

            Map(x => x.Sort);
            
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }

    }
}
