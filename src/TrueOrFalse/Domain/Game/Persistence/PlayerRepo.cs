using NHibernate;

public class PlayerRepo :  RepositoryDbBase<Player>
{
    public PlayerRepo(ISession session) : base(session)
    {
    }
}
