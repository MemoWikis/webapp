using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class MainCategoryMap : ClassMap<MainCategory>
    {
        public MainCategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
