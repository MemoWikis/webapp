using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionMap : ClassMap<Question>
    {
        public QuestionMap()
        {
            Id(x => x.Id);
            Map(x => x.Text).Length(Constants.VarCharMaxLength);
            Map(x => x.Visibility);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
            HasMany<Answer>(x => x.Answers).Cascade.All();
        }
    }
}
