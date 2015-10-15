using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;

public class UserActivityRepo : RepositoryDb<UserActivity> 
{
    public UserActivityRepo(ISession session) : base(session) { }


    public IList<UserActivity> GetByUser(User user, int amount = 10)
    {
        return Query
            .Where(x => x.UserConcerned == user)
            .OrderBy(x => x.At).Desc
            .Take(amount)
            .List<UserActivity>();
    }
}