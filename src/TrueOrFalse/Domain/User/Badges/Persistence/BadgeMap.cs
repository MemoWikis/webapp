using FluentNHibernate.Mapping;

public class BadgeMap : ClassMap<Badge>
{
    public BadgeMap()
    {
        Id(x => x.Id);

        Map(x => x.BadgeTypeKey);
        Map(x => x.TimesGiven);

        Map(x => x.Points);
        Map(x => x.Level);

        References(x => x.User);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}