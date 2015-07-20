using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class AnswerHistoryRepository : RepositoryDb<AnswerHistory> 
{
    public ISession Session { get { return _session; } }

    public AnswerHistoryRepository(ISession session) : base(session){}

    public void DeleteFor(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM AnswerHistory ah WHERE ah.QuestionId = :questionId")
                .SetParameter("questionId", questionId);
    }

    public IList<AnswerHistory> GetBy(List<int> questionsId, int userId)
    {
        return Session.QueryOver<AnswerHistory>()
            .Where(Restrictions.In("QuestionId", questionsId))
            .And(q => q.UserId == userId)
            .List();
    }

    public IList<AnswerHistory> GetBy(int questionId)
    {
        return Session.QueryOver<AnswerHistory>()
                        .Where(i => i.QuestionId == questionId)
                        .List<AnswerHistory>();
    }

    public IList<AnswerHistory> GetBy(int questionId, int userId)
    {
        return Session.QueryOver<AnswerHistory>()
                        .Where(i => i.QuestionId == questionId && i.UserId == userId)
                        .List<AnswerHistory>();
    }

    public override void Create(AnswerHistory answerHistory)
    {
        _session.Save(answerHistory);
    }
}