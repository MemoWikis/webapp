using System.Diagnostics;
using NHibernate;
using NHibernate.Criterion;

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

    public IEnumerable<QuestionView> GetAllTodayViews()
    {
        var watch = new Stopwatch();
        watch.Start();
        var query = _session.CreateCriteria<QuestionView>()
            .Add(Restrictions.Ge("DateCreated", DateTime.Now.Date));

        var result = query.List<QuestionView>();  
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        return result;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }
}