using NHibernate;


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
            LEFT JOIN pages_to_questions cq
            ON cq.Question_id = q.Id
            WHERE cq.Page_id = " + pageId;

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

    public IList<DailyActivityCount> GetDailyActivityForUser(int userId, DateTime startDate)
    {
        return Session.CreateSQLQuery(@"
                SELECT DATE(DateCreated) AS Day, COUNT(*) AS Count
                FROM answer
                WHERE UserId = :userId
                  AND DateCreated >= :startDate
                  AND AnswerredCorrectly != :isView
                GROUP BY DATE(DateCreated)
                ORDER BY Day")
            .SetParameter("userId", userId)
            .SetParameter("startDate", startDate)
            .SetParameter("isView", (int)AnswerCorrectness.IsView)
            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<DailyActivityCount>())
            .List<DailyActivityCount>();
    }

    public IList<DailyActivityCount> GetDailyActivityForUserOnPage(int userId, int pageId, DateTime startDate)
    {
        return Session.CreateSQLQuery(@"
                SELECT DATE(ah.DateCreated) AS Day, COUNT(*) AS Count
                FROM answer ah
                JOIN pages_to_questions cq ON cq.Question_id = ah.QuestionId
                WHERE ah.UserId = :userId
                  AND cq.Page_id = :pageId
                  AND ah.DateCreated >= :startDate
                  AND ah.AnswerredCorrectly != :isView
                GROUP BY DATE(ah.DateCreated)
                ORDER BY Day")
            .SetParameter("userId", userId)
            .SetParameter("pageId", pageId)
            .SetParameter("startDate", startDate)
            .SetParameter("isView", (int)AnswerCorrectness.IsView)
            .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<DailyActivityCount>())
            .List<DailyActivityCount>();
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