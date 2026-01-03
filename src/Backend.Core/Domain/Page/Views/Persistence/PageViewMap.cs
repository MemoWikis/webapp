using FluentNHibernate.Mapping;

public class PageViewMap : ClassMap<PageView>
{
    public PageViewMap()
    {
        Table("pageview");

        Id(x => x.Id);

        References(x => x.Page).Column("Page_id").Cascade.None().Not.ForeignKey();
        References(x => x.User).Cascade.None();

        Map(x => x.UserAgent);

        Map(x => x.DateCreated);
        Map(x => x.DateOnly).ReadOnly();
    }
}