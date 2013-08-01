using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class SetRepository : RepositoryDb<Set>
    {
        public SetRepository(ISession session)
            : base(session){}

    }
}
