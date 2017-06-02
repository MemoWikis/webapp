using NHibernate;
using Seedworks.Lib.Persistence;

public class ActivityRewardRepo : RepositoryDb<ActivityReward>
{
    public ActivityRewardRepo(ISession session) : base(session)
    {
    }

    //public IList<Message> GetForUser(int userId)
    //{
    //    return _session.QueryOver<Message>()
    //        .Where(x => x.ReceiverId == userId)
    //        .OrderBy(x => x.DateCreated).Desc
    //        .List<Message>();
    //}
}