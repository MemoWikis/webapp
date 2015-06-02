using NHibernate;

public class DateRepo : RepositoryDbBase<Date>
{
    public DateRepo(ISession session) : base(session)
    {
    }
}