using NHibernate;
using Seedworks.Lib.Persistence;

public class AnswerRepo : RepositoryDb<Answer>
{
    public AnswerRepo(ISession session) : base(session)
    {
    }

    public override void Create(Answer answer)
    {
        _session.Save(answer);
    }

    public void DeleteFor(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM answer WHERE answer.QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public IList<Answer> GetByPages(int pageId, bool includingSolutionViews = false)
    {
        var query = @"
            SELECT ah.Id FROM answer ah
            LEFT JOIN question q
            ON q.Id = ah.QuestionId
            LEFT JOIN categories_to_questions cq
            ON cq.Question_id = q.Id
            WHERE cq.Category_id = " + pageId;

        var ids = Session.CreateSQLQuery(query).List<int>();

        if (includingSolutionViews)
        {
            return GetByIds(ids.ToArray());
        }

        return GetByIds(ids.ToArray()).Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView)
            .ToList();
    }

    public IList<Answer> GetByQuestion(int questionId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.Question.Id == questionId)
            .List<Answer>();
    }

    public IList<Answer> GetByQuestion(
        int questionId,
        int userId,
        bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.Question.Id == questionId && i.UserId == userId)
            .List();
    }

    public IList<Answer> GetByQuestionViewGuid(Guid questionViewGuid)
    {
        return questionViewGuid == default
            ? null
            : _session.QueryOver<Answer>()
                .Where(a => a.QuestionViewGuidString == questionViewGuid.ToString())
                .List<Answer>();
    }

    public IList<Answer> GetByUser(int userId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.UserId == userId)
            .List<Answer>();
    }

    public Answer GetLastCreated(bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .OrderBy(x => x.DateCreated).Desc
            .Take(1)
            .SingleOrDefault();
    }

    private new IQueryOver<Answer, Answer> Query(bool includingSolutionViews = false)
    {
        var query = Session.QueryOver<Answer>();

        if (!includingSolutionViews)
        {
            query.Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView);
        }

        return query;
    }
}