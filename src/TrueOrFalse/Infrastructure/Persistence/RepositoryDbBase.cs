using NHibernate;
using Seedworks.Lib.Persistence;

public class RepositoryDbBase<T> : RepositoryDb<T> where T : class, IPersistable
{
    protected SessionUserLegacy UserLegacySession
    {
        get { return Sl.R<SessionUserLegacy>(); }
    }

    public RepositoryDbBase(ISession session) : base(session)
    {
    }
}
