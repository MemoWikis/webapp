﻿using FluentNHibernate.Mapping;

public class CategoryRelationMap : ClassMap<CategoryRelation>
{
    public CategoryRelationMap()
    {
        Table("relatedcategoriestorelatedcategories");
        Id(x => x.Id);

        References(x => x.Child).Column("Category_id").Cascade.None();
        References(x => x.Parent).Column("Related_id").Cascade.None();
        Map(x => x.PreviousId).Column("Previous_id").Nullable();
        Map(x => x.NextId).Column("Next_id").Nullable();
    }
}
