using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;

public class ActivityPointsRepo : RepositoryDb<ActivityPoints>
{
    public ActivityPointsRepo(ISession session) : base(session)
    {
    }

    public IList<ActivityPoints> GetActivtyPointsByUser(int userId)
    {
        return Session.QueryOver<ActivityPoints>()
            .Where(x => x.User.Id == userId)
            .List();
    }
}