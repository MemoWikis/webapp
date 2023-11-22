using System.CodeDom;
using NHibernate;

public class TotalFollowers : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;

    public TotalFollowers(ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
    }
    public int Run(int userId)
    {
        return Convert.ToInt32(_nhibernateSession.CreateSQLQuery(
            @" SELECT count(User_id)
               FROM user_to_follower
               WHERE User_id = " + userId
        ).UniqueResult<object>());
    }
}