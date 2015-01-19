using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class RepositoryDbBase<T> : RepositoryDb<T> where T : class, IPersistable
{
    protected SessionUser _userSession
    {
        get { return Sl.R<SessionUser>(); }
    }

    public RepositoryDbBase(ISession session) : base(session)
    {
    }

    protected void ThrowIfNot_IsUserOrAdmin(int id)
    {
        global::ThrowIfNot_IsUserOrAdmin.Run(id);
    }
}
