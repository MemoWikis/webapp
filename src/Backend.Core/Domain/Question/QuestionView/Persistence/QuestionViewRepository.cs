using NHibernate;
using NHibernate.Criterion;
using MessagePack;
using System.Collections.Concurrent;
using System.Diagnostics;

public class QuestionViewRepository(ISession _session, QuestionViewMmapCache questionViewMmapCache) : RepositoryDbBase<QuestionView>(_session)
{
    public int GetViewCount(int questionId)
    {
        return _session.QueryOver<QuestionView>()
            .Select(Projections.RowCount())
            .Where(x => x.QuestionId == questionId)
            .FutureValue<int>()
            .Value;
    }

    public ConcurrentDictionary<DateTime, int> GetViewsForPastNDays(int days)
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
        Log.Information("GetViewsForLastNDays took " + elapsed + "ms");

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<QuestionViewSummaryWithId> GetViewsForLastNDaysGroupByQuestionId(int days)
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
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Log.Information("GetViewsForLastNDaysGroupByQuestionId " + elapsed);

        return result;
    }

    public IList<QuestionViewSummary> GetViewsForPastNDaysByIds(int days, List<int> ids)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly 
        FROM QuestionView 
        WHERE QuestionId IN (:questionIds) AND DateOnly BETWEEN NOW() - INTERVAL :days DAY AND NOW()
        GROUP BY DateOnly");

        query.SetParameterList("questionIds", ids);
        query.SetParameter("days", days);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummary)))
            .List<QuestionViewSummary>();

        return result;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public record struct QuestionViewSummary(Int64 Count, DateTime DateOnly);

    public IList<QuestionViewSummaryWithId> GetAllEager()
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, QuestionId, MAX(DateCreated) as DateCreated
        FROM QuestionView 
        GROUP BY 
            QuestionId, 
            DateOnly
        ORDER BY 
            QuestionId, 
            DateOnly;");

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();

        return result;
    }

    public IList<QuestionViewSummaryWithId> GetAllEagerSince(DateTime sinceDate)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, QuestionId, MAX(DateCreated) as DateCreated
        FROM QuestionView 
        WHERE DateCreated > :sinceDate
        GROUP BY 
            QuestionId, 
            DateOnly
        ORDER BY 
            QuestionId, 
            DateOnly;");

        query.SetParameter("sinceDate", sinceDate);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();

        return result;
    }

    public void AddView(int questionId, int userId)
    {
        var questionView = new QuestionView
        {
            QuestionId = questionId,
            UserId = userId,
            DateCreated = DateTime.UtcNow,
            DateOnly = DateTime.UtcNow.Date
        };

        Create(questionView);

        // TODO: Add to mmap cache
        // questionViewMmapCache.AppendQuestionView(new QuestionViewSummaryWithId(
        //     1, questionView.DateOnly, questionId, questionView.DateCreated));
    }
}