using NHibernate;
using Seedworks.Lib.Persistence;

public class RepositoryDbBase<T> : RepositoryDb<T> where T : class, IPersistable
{
    public RepositoryDbBase(ISession session) : base(session)
    {
    }
}
