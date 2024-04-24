using NHibernate;

public class FollowerCounts : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;

    public FollowerCounts(ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
    }

    private readonly Dictionary<int, int> _followers = new Dictionary<int, int>();
    private bool _initialized;

    public FollowerCounts Init(IEnumerable<int> userIds)
    {
        _initialized = true;

        var query = @"
            SELECT Count(User_id), User_id
            FROM user_to_follower
            WHERE User_id IN ({0})
            GROUP BY User_id
        ";

        query = String.Format(query,
            userIds
                .Select(u => u.ToString())
                .Aggregate((a, b) => a + "," + b));

        var listOfObjects = _nhibernateSession
            .CreateSQLQuery(query)
            .List<object[]>();

        foreach (var item in listOfObjects)
            _followers.Add(Convert.ToInt32(item[1]), Convert.ToInt32(item[0]));

        return this;
    }

    public int ByUserId(int userId)
    {
        if (!_initialized)
            throw new Exception("call Init(IList<int> userIds) first");

        if (!_followers.ContainsKey(userId))
            return 0;

        return _followers[userId];
    }
}