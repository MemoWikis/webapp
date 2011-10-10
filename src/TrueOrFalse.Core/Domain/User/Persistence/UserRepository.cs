using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class UserRepository : RepositoryDb<User>
    {
        public UserRepository(ISession session): base(session)
        {
        }

        public User GetByEmailAddress(string emailAddress)
        {
            var userSearchSpec = new UserSearchSpec();
            userSearchSpec.Filter.EmailAddress.EqualTo(emailAddress);
            return GetByUnique(userSearchSpec);
        }
    }
}
