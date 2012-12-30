using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            References(x => x.Creator);
            Map(x => x.QuestionCount);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
            HasManyToMany(x => x.RelatedCategories).ChildKeyColumn("Related_Id").Cascade.DeleteOrphan();
        }
    }
}
