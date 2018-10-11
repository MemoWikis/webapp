using NHibernate;


public class GetWishTopicCount : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetWishTopicCount(ISession session)
    {
        _session = session;
    }

    public int Run(int userId)
    {
        return (int)_session.CreateQuery(
                "SELECT count(distinct cv.Id) FROM CategoryValuation cv " +
                "WHERE UserId = " + userId +
                "AND RelevancePersonal > -1 ")
            .UniqueResult<long>();
    }
}

