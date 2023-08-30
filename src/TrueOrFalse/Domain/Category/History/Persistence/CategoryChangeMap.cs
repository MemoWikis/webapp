using FluentNHibernate.Mapping;

public class CategoryChangeMap : ClassMap<CategoryChange>
{
    public CategoryChangeMap()
    {
        Id(x => x.Id);

        References(x => x.Category).NotFound.Ignore();

        Map(x => x.Data);
        Map(x => x.ShowInSidebar);

        Map(x => x.DataVersion);
        Map(x => x.Type);

        Map(x => x.AuthorId).Column("Author_id");

        Map(x => x.DateCreated);
    }
}

