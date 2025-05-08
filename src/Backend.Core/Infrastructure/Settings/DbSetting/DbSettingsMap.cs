using FluentNHibernate.Mapping;

public class DbSettingsMap : ClassMap<DbSettings>
{
    public DbSettingsMap()
    {
        Table("Setting");
        Id(x => x.Id);
        Map(x => x.AppVersion);
        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}