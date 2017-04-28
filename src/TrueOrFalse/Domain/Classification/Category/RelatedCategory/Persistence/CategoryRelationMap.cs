using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

//public class CategoryRelationMap : ClassMap<CategoryRelation>
//{
//    public CategoryRelationMap()
//    {
//        Table("relatedcategoriestorelatedcategories");

//        References(x => x.Category).Cascade.None();
//        References(x => x.RelatedCategory).Cascade.None();

//        Map(x => x.RelationType);

//        Map(x => x.DateCreated);
//        Map(x => x.DateModified);
//    }
//}
