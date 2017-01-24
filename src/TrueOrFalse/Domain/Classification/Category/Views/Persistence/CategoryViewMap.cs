using FluentNHibernate.Mapping;

public class CategoryViewMap : ClassMap<CategoryView>
{
    public CategoryViewMap()
    {
        Id(x => x.Id);

        References(x => x.Category).Cascade.None();
        References(x => x.User).Cascade.None();

        Map(x => x.UserAgent);

        Map(x => x.DateCreated);
    }
}