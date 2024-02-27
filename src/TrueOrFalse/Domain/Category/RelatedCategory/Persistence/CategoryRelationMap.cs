using FluentNHibernate.Mapping;

public class CategoryRelationMap : ClassMap<CategoryRelation>
{
    public CategoryRelationMap()
    {
        Table("relatedcategoriestorelatedcategories");
        Id(x => x.Id);

        References(x => x.Child).Column("Category_id").Cascade.None();
        References(x => x.Parent).Column("Related_id").Cascade.None();
    }
}
