using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class QuestionMap : ClassMap<Question>
    {
        public QuestionMap()
        {
            Id(x => x.Id);
            Map(x => x.Text);
        }
    }
}
