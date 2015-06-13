using NHibernate;

public class RoundRepo :  RepositoryDbBase<Round>
{
    public RoundRepo(ISession session) : base(session)
    {
    }
}
