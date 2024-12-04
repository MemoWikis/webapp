using FluentNHibernate.Mapping;

public class PageChangeMap : ClassMap<PageChange>
{
    public PageChangeMap()
    {
        Table("pagechange");

        Id(x => x.Id);

        References(x => x.Page).Column("Page_id").NotFound.Ignore();

        Map(x => x.Data).CustomSqlType("longtext");
        Map(x => x.ShowInSidebar);

        Map(x => x.DataVersion);
        Map(x => x.Type).CustomType<PageChangeType>();

        Map(x => x.AuthorId).Column("Author_id");

        Map(x => x.DateCreated);
    }
}

