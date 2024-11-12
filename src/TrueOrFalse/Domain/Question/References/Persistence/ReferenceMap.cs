using FluentNHibernate.Mapping;

public class ReferenceMap : ClassMap<Reference>
{
    public ReferenceMap()
    {
        Id(x => x.Id);

        References(x => x.Question).Cascade.None();
        References(x => x.Page).Column("Category_id");

        Map(x => x.ReferenceType).CustomType<ReferenceType>();
        Map(x => x.AdditionalInfo);
        Map(x => x.ReferenceText);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}