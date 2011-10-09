using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Core
{
    public class AnswerMap : ClassMap<Answer>
    {
        public AnswerMap()
        {
            Id(x => x.Id);
            Map(x => x.Type);
            Map(x => x.Text).Length(Constants.VarCharMaxLength);
            Map(x => x.Description).Length(Constants.VarCharMaxLength);
            References(x => x.Creator);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
            References(x => x.Question);
        }

    }
}
