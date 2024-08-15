﻿using System.Collections.Concurrent;
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

    public ConcurrentDictionary<DateTime, int> GetViewsForLastNDays(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery("SELECT COUNT(DateOnly) AS Count, DateOnly FROM QuestionView WHERE DateOnly BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() GROUP BY DateOnly");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummary)))
            .List<QuestionViewSummary>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();

        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public class QuestionViewSummary
    {
        public Int64 Count { get; set; }
        public DateTime DateOnly { get; set; }
    }

}