using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class UserRepository : RepositoryDb<User>
    {
        public UserRepository(ISession session): base(session){}

        public User GetByEmail(string emailAddress)
        {
            return _session.QueryOver<User>()
                           .Where(k => k.EmailAddress == emailAddress)
                           .SingleOrDefault<User>();
        }
    }
}
