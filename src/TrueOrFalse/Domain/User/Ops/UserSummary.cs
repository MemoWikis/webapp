using NHibernate;
using NHibernate.Criterion;

public class UserSummary : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;

    public UserSummary(ISession nhibernateSession)
    {
        _nhibernateSession = nhibernateSession;
    }
    public int AmountCreatedQuestions(int creatorId, bool inclPrivateQuestions = true)
    {
        var query = _nhibernateSession
            .QueryOver<Question>()
            .Select(Projections.RowCount())
            .Where(q => q.Creator != null && q.Creator.Id == creatorId);

        if (!inclPrivateQuestions)
            query = query.Where(q => q.Visibility == QuestionVisibility.All);

        return query.FutureValue<int>().Value;
    }

    public int AmountCreatedCategories(int creatorId, bool inclPrivateCategories = true)
    {
        var query = _nhibernateSession
            .QueryOver<Page>()
            .Select(Projections.RowCount())
            .Where(c => c.Creator != null && c.Creator.Id == creatorId);

        if (!inclPrivateCategories)
            query = query.Where(q => q.Visibility == PageVisibility.All);

        return query.FutureValue<int>().Value;
    }

}