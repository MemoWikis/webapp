using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            HasMany(x => x.SubCategories).Cascade.All();
            References(x => x.Creator);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
            
        }
    }
}
