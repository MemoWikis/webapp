using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class AnswerHistoryRepo : RepositoryDb<AnswerHistory> 
{
    public AnswerHistoryRepo(ISession session) : base(session){}

    public void DeleteFor(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM AnswerHistory ah WHERE ah.QuestionId = :questionId")
                .SetParameter("questionId", questionId);
    }

    public IList<AnswerHistory> GetByQuestion(List<int> questionsId, int userId)
    {
        return Session.QueryOver<AnswerHistory>()
            .Where(Restrictions.In("QuestionId", questionsId))
            .And(q => q.UserId == userId)
            .List();
    }

    public IList<AnswerHistory> GetByQuestion(int questionId)
    {
        return Session.QueryOver<AnswerHistory>()
                        .Where(i => i.QuestionId == questionId)
                        .List<AnswerHistory>();
    }

    public IList<AnswerHistory> GetByQuestion(int questionId, int userId)
    {
        return Session.QueryOver<AnswerHistory>()
                        .Where(i => i.QuestionId == questionId && i.UserId == userId)
                        .List<AnswerHistory>();
    }

    public IList<AnswerHistory> GetByCategories(int categoryId)
    {
        string query = @"
            SELECT ah.Id FROM answerhistory ah
            LEFT JOIN question q
            ON q.Id = ah.QuestionId
            LEFT JOIN categoriestoquestions cq
            ON cq.Question_id = q.Id
            WHERE cq.Category_id = " + categoryId;

        var ids = Session.CreateSQLQuery(query).List<int>();
        return GetByIds(ids.ToArray());
    }

    public IList<AnswerHistory> GetByUsers(int userId)
    {
        return Session.QueryOver<AnswerHistory>()
            .Where(i => i.UserId == userId)
            .List<AnswerHistory>();
    }

    public override void Create(AnswerHistory answerHistory)
    {
        _session.Save(answerHistory);
    }

    public IList<AnswerHistory> GetAllEager()
    {
        return Session.QueryOver<AnswerHistory>()
            .Fetch(x => x.Round).Eager
            .Fetch(x => x.Player).Eager
            .List();
    }
}