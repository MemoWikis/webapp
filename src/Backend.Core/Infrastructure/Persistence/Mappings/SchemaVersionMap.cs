using FluentNHibernate.Mapping;

public class SchemaVersionMap : ClassMap<SchemaVersion>
{
    public SchemaVersionMap()
    {
        Table("SchemaVersion");
        Id(x => x.Id);
        Map(x => x.SchemaHash).Length(64);
        Map(x => x.LastUpdated);
    }
}
