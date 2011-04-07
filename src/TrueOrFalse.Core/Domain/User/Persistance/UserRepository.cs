using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class UserRepository : RepositoryDb<User, UserList>
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
