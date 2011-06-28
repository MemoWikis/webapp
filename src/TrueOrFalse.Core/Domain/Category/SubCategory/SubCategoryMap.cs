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
            Map(x => x.MainCategory);
            Map(x => x.Title);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
