using NHibernate;


public class RepositoryDbBase<T> : RepositoryDb<T> where T : class, IPersistable
{
    public RepositoryDbBase(ISession session) : base(session)
    {
    }
}
