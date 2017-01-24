using FluentNHibernate.Mapping;

public class SetViewMap : ClassMap<SetView>
{
    public SetViewMap()
    {
        Id(x => x.Id);

        References(x => x.Set).Cascade.None();
        References(x => x.User).Cascade.None();

        Map(x => x.UserAgent);

        Map(x => x.DateCreated);
    }
}