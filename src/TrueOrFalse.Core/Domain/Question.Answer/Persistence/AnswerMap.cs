using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class AnswerMap : ClassMap<Answer>
    {
        public AnswerMap()
        {
            Id(x => x.Id);
            Map(x => x.Type);
            Map(x => x.Text);
            Map(x => x.Description);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
            HasManyToMany<Question>(x => x.Questions)
                .Table("QuestionAnswer").Inverse();
        }

    }
}
