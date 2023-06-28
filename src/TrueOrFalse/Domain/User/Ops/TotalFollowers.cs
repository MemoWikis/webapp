using NHibernate;

public class TotalFollowers : IRegisterAsInstancePerLifetime
{
    public int Run(int userId)
    {
        return Convert.ToInt32(Sl.R<ISession>().CreateSQLQuery(
            @" SELECT count(User_id)
               FROM user_to_follower
               WHERE User_id = " + userId
        ).UniqueResult<object>());
    }
}