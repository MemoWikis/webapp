using NHibernate;
using NHibernate.Criterion;
using System.Collections.Concurrent;
using System.Diagnostics;

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

    public ConcurrentDictionary<DateTime, int> GetViewsForLastNDays(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery(@"
            SELECT 
                COUNT(DateOnly) AS Count, 
                DateOnly 
            FROM QuestionView 
            WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() 
            GROUP BY DateOnly");

        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummary)))
            .List<QuestionViewSummary>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Logg.r.Information("GetViewsForLastNDays took " + elapsed + "ms");

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<QuestionViewSummaryOrderById> GetViewsForLastNDaysGroupByCategoryId(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery(@"
            SELECT 
                QuestionId, 
                DateOnly, 
                COUNT(DateOnly) AS Count 
            FROM QuestionView 
            WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() 
            GROUP BY 
                QuestionId, 
                DateOnly 
            ORDER BY 
                QuestionId, 
                DateOnly;");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryOrderById)))
            .List<QuestionViewSummaryOrderById>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Logg.r.Information("GetViewsForLastNDaysGroupByQuestionId " + elapsed);

        return result;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public record struct QuestionViewSummary(Int64 Count, DateTime DateOnly);
    public record struct QuestionViewSummaryOrderById(Int64 Count, DateTime DateOnly, int QuestionId);
}