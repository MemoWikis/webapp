using FluentNHibernate.Mapping;

public class CategoryChangeMap : ClassMap<CategoryChange>
{
    public CategoryChangeMap()
    {
        Id(x => x.Id);

        References(x => x.Category); ;

        Map(x => x.Data);
        Map(x => x.ShowInSidebar);

        Map(x => x.DataVersion);
        Map(x => x.Type);

        References(x => x.Author);

        Map(x => x.DateCreated);
    }
}

