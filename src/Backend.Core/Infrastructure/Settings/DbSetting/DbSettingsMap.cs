using FluentNHibernate.Mapping;

public class DbSettingsMap : ClassMap<DbSettings>
{
    public DbSettingsMap()
    {
        Table("Setting");
        Id(x => x.Id);
        Map(x => x.AppVersion);
        Map(x => x.SuggestedGames);
        Map(x => x.SuggestedSetsIdString);
        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}