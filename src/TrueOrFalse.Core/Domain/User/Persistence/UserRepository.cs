using NHibernate;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class UserRepository : RepositoryDb<User>
    {
        public UserRepository(ISession session): base(session)
        {
        }

        public User GetByUserName(string userName)
        {
            var userSearchSpec = new UserSearchSpec();
            userSearchSpec.Filter.UserName.EqualTo(userName);
            return GetByUnique(userSearchSpec);
        }
    }
}
