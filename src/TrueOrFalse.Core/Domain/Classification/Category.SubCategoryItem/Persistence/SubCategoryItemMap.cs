using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class SubCategoryItemMap : ClassMap<SubCategoryItem>
    {
        public SubCategoryItemMap()
        {
            Id(x => x.Id);
            References(x => x.SubCategory);
            Map(x => x.Name);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
