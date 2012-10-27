using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class UserRepository : RepositoryDb<User>
    {
        public UserRepository(ISession session): base(session){}

        public User GetByEmailAddress(string emailAddress)
        {
            var userSearchSpec = new UserSearchSpec();
            userSearchSpec.Filter.EmailAddress.EqualTo(emailAddress);
            return GetByUnique(userSearchSpec);
        }

        public User GetByEmail(string emailAddress)
        {
            return _session.QueryOver<User>()
                           .Where(k => k.EmailAddress == emailAddress)
                           .SingleOrDefault<User>();
        }
    }
}
