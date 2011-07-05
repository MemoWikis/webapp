using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class KeywordMap : ClassMap<Keyword>
    {
        public KeywordMap()
        {
            Id(x => x.Id);
            References(x => x.Category);
            Map(x => x.Name);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
