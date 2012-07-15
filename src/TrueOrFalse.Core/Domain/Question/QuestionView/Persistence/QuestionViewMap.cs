using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class QuestionViewMap : ClassMap<QuestionView>
    {
        public QuestionViewMap()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.QuestionId);
            Map(x => x.DateCreated);
        }
    }
}
