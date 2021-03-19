using FluentNHibernate.Mapping;

public class CategoryRelationMap : ClassMap<CategoryRelation>
{
    public CategoryRelationMap()
    {
        Table("relatedcategoriestorelatedcategories");
        Id(x => x.Id);

        References(x => x.CategoryId).Cascade.None();
        References(x => x.RelatedCategoryId).Column("Related_id").Cascade.None();

        Map(x => x.CategoryRelationType);
    }
}
