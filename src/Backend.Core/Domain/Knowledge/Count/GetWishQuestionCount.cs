using NHibernate;

public class GetWishQuestionCount : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetWishQuestionCount(ISession session){
        _session = session;
    }

    public int Run(int userId)
    {
        return (int)_session.CreateQuery(
            "SELECT count(qv) FROM QuestionValuation qv " +
            "WHERE UserId = " + userId +
            "AND RelevancePersonal > -1 ").UniqueResult<Int64>();
    }
}