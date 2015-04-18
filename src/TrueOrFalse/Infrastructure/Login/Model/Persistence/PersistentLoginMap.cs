using FluentNHibernate.Mapping;

public class PersistentLoginMap : ClassMap<PersistentLogin>
{
    public PersistentLoginMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        Map(x => x.LoginGuid);
        Map(x => x.Created);
    }
}