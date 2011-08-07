using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class SubCategoryMap : ClassMap<SubCategory>
    {
        public SubCategoryMap()
        {
            Id(x => x.Id);
            References(x => x.Category);
            HasMany(x => x.Items).Cascade.All();
            Map(x => x.Name);
            Map(x => x.Type);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
