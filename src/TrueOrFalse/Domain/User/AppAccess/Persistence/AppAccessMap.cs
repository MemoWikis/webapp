using FluentNHibernate.Mapping;

public class AppAccessMap : ClassMap<AppAccess>
{
    public AppAccessMap()
    {
        Id(x => x.Id);
        References(x => x.User);

        Map(x => x.AppKey);
        Map(x => x.AccessToken);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}
