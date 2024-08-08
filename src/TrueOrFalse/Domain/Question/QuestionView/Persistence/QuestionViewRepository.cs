using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class QuestionViewRepository(ISession _session) : RepositoryDbBase<QuestionView>(_session)
{

    public int GetViewCount(int questionId)
    {
        return _session.QueryOver<QuestionView>()
            .Select(Projections.RowCount())
            .Where(x => x.QuestionId == questionId)
            .FutureValue<int>()
            .Value;
    }
    public void AddView(string userAgent, int questionId, int userId)
    {
        var questionView = new QuestionView
        {
           DateCreated = DateTime.Now,
           QuestionId = questionId,
           UserAgent = userAgent,
           UserId = userId
        };

        using (var transaction = _session.BeginTransaction())
        {
            _session.Save(questionView);
            transaction.Commit();
        }

        EntityCache.GetQuestionById(questionId).TodayViewCount ++;
    }

    public int GetTodayViewCount(int questionId)
    {
        return _session.QueryOver<QuestionView>()
            .Select(Projections.RowCount())
            .Where(x => x.QuestionId == questionId &&
                        DateTime.Now.Date == x.DateCreated)
            .FutureValue<int>()
            .Value;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }
}