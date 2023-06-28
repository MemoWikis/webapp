using NHibernate;

public class TotalIFollow : IRegisterAsInstancePerLifetime
{
    public int Run(int userId)
    {
        var result = Sl.R<ISession>().CreateSQLQuery(
            @" SELECT count(User_id)
               FROM user_to_follower
               WHERE Follower_id = " + userId
        ).UniqueResult<object>();

        return Convert.ToInt32(result);
    }
}