using FluentNHibernate.Mapping;

public class CategoryChangeMap : ClassMap<CategoryChange>
{
    public CategoryChangeMap()
    {
        Id(x => x.Id);

        Map(x => x.Data);
        Map(x => x.DataVersion);

        Map(x => x.Author);

        Map(x => x.DateCreated);
    }
}

