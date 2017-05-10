using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Testing.Values;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class QuestionViewRepository : RepositoryDb<QuestionView>
{
    public QuestionViewRepository(ISession session) : base(session) { }

    public int GetViewCount(int questionId)
    {
        return _session.QueryOver<QuestionView>()
            .Select(Projections.RowCount())
            .Where(x => x.QuestionId == questionId)
            .FutureValue<int>()
            .Value;
    }

    public List<AmountPerDay> GetViewsPerDayForSetOfQuestions(List<int> questionIds)
    {
        return _session.QueryOver<QuestionView>()
            .Where(x => x.QuestionId.IsIn(questionIds.ToArray()))
            .List()
            .GroupBy(x => x.DateCreated.Date)
            .Select(x => new AmountPerDay
            {
                DateTime = x.Key,
                Value = x.Count()
            })
            .ToList();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public QuestionView GetByGuid(Guid guid)
    {
        return _session.QueryOver<QuestionView>()
            .Where(x => x.GuidString == guid.ToString())
            .SingleOrDefault<QuestionView>();
    }
}