using NHibernate;
using Seedworks.Lib.Persistence;

public class RepositoryDbBase<T> : RepositoryDb<T> where T : class, IPersistable
{
    protected SessionUser _userSession
    {
        get { return Sl.R<SessionUser>(); }
    }

    public RepositoryDbBase(ISession session) : base(session)
    {
    }
}
