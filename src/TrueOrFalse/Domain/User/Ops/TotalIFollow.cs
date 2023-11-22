using NHibernate;

public class TotalIFollow : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;

    public TotalIFollow(ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
    }
    public int Run(int userId)
    {
        var result = _nhibernateSession.CreateSQLQuery(
            @" SELECT count(User_id)
               FROM user_to_follower
               WHERE Follower_id = " + userId
        ).UniqueResult<object>();

        return Convert.ToInt32(result);
    }
}