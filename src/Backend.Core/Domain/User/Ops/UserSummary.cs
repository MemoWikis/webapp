using NHibernate;
using NHibernate.Criterion;

public class UserSummary : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;

    public UserSummary(ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
    }
    public int GetCreatedQuestionCount(int creatorId, bool includePrivateQuestions = true)
    {
        var query = _nhibernateSession
            .QueryOver<Question>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator != null && q.Creator.Id == creatorId);

        if (!includePrivateQuestions)
            query = query.Where(q => q.Visibility == QuestionVisibility.Public);

        return query.FutureValue<int>().Value;
    }

    public int GetCreatedPagesCount(int creatorId, bool includePrivatePages = true)
    {
        var query = _nhibernateSession
            .QueryOver<Page>()
            .Select(Projections.RowCount())
            .Where(c => c.Creator != null && c.Creator.Id == creatorId);

        if (!includePrivatePages)
            query = query.Where(q => q.Visibility == PageVisibility.Public);

        return query.FutureValue<int>().Value;
    }

}