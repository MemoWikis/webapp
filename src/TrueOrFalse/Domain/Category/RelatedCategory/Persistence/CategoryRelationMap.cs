using FluentNHibernate.Mapping;

public class CategoryRelationMap : ClassMap<CategoryRelation>
{
    public CategoryRelationMap()
    {
        Table("relatedcategoriestorelatedcategories");
        Id(x => x.Id);

        References(x => x.Category).Cascade.None();
        References(x => x.RelatedCategory).Column("Related_id").Cascade.None();

        Map(x => x.CategoryRelationType);
    }
}
