using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class AnswerRepo : RepositoryDb<Answer> 
{
    public AnswerRepo(ISession session) : base(session){}

    public void DeleteFor(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM answer WHERE answer.QuestionId = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    private new IQueryOver<Answer, Answer> Query(bool includingSolutionViews = false)
    {
        var query = Session.QueryOver<Answer>();

        if (!includingSolutionViews)
            query.Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView);

        return query;
    }

    public IList<Answer> GetByQuestion(List<int> questionsId, int userId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(Restrictions.In("Question.Id", questionsId))
            .And(a => a.UserId == userId)
            .List<Answer>();
    }

    public IList<Answer> GetByQuestion(int questionId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.Question.Id == questionId)
            .List<Answer>();
    }

    public IList<Answer> GetByQuestion(int questionId, int userId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.Question.Id == questionId && i.UserId == userId)
            .List();
    }

    public IList<Answer> GetByFeatures(AnswerFeature answerFeature, QuestionFeature questionFeature, bool includingSolutionViews = false)
    {
        var query = Query(includingSolutionViews);

        if (answerFeature != null)
        {
            AnswerFeature answerFeatureAlias = null;
            query = query
                .JoinAlias(x => x.Features, () => answerFeatureAlias)
                .Where(x => answerFeatureAlias.Id == answerFeature.Id);
        }

        if (questionFeature != null)
        {
            Question questionAlias = null;
            QuestionFeature questionFeatureAlias = null;
            query = query
                .JoinAlias(x => x.Question, () => questionAlias)
                .JoinAlias(x => questionAlias.Features, () => questionFeatureAlias)
                .Where(x => questionFeatureAlias.Id == questionFeature.Id);
        }

        return query.List<Answer>();
    }

    public IList<Answer> GetByCategories(int categoryId, bool includingSolutionViews = false)
    {
        string query = @"
            SELECT ah.Id FROM answer ah
            LEFT JOIN question q
            ON q.Id = ah.QuestionId
            LEFT JOIN categories_to_questions cq
            ON cq.Question_id = q.Id
            WHERE cq.Category_id = " + categoryId;

        var ids = Session.CreateSQLQuery(query).List<int>();

        if(includingSolutionViews)
            return GetByIds(ids.ToArray());

        return GetByIds(ids.ToArray()).Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView).ToList();
    }

    public IList<Answer> GetByUser(int userId, bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Where(i => i.UserId == userId)
            .List<Answer>();
    }

    /// <summary>
    /// returns last #amount of questions a user has interacted with (no matter if answered or only solution viewed), 
    /// without showing a question twice.
    /// If a question is interacted with more than once, only the most recent interaction is considered.
    /// </summary>
    public IList<Answer> GetUniqueByUser(int userId, int amount)
    {
        //Older version, does not sort out duplicate entrys:
        //return Sl.R<ISession>()
        //    .QueryOver<Answer>()
        //    .Where(a => a.UserId == userId)
        //    .OrderBy(a => a.DateCreated).Desc
        //    .Take(amount)
        //    .List<Answer>();

        string query = @"
            SELECT MAX(id) as id FROM answer
            WHERE UserId = " + userId + @"
            GROUP BY QuestionId
            ORDER BY MAX(DateCreated) DESC 
            LIMIT " + amount;
        var ids = Session.CreateSQLQuery(query).List<int>();

        return GetByIds(ids.ToArray()).OrderByDescending(a => a.DateCreated).ToList();

        //in one query, but doesn't fit type <Answer>, even though fields match 1:1
        //string query = @"
        //    SELECT a2.* FROM answer a2
        //    INNER JOIN (
        //        SELECT MAX(id) as maxId FROM answer
        //        WHERE UserId = " + userId + @"
        //        GROUP BY QuestionId
        //        ORDER BY MAX(DateCreated) DESC 
        //        LIMIT " + amount + @"
        //    ) a1
        //    ON a1.maxId = a2.Id";
        //return Session.CreateSQLQuery(query).List<Answer>();
    }

    public IList<Answer> GetByQuestionViewGuids(List<Guid> questionViewGuids, bool excludeSolutionViews)
    {
        if (questionViewGuids.All(g => g == default(Guid)))
            return null;

        var questionViewGuidsStrings = questionViewGuids.ConvertAll(g => Convert.ToString(g));
        var result = _session.QueryOver<Answer>()
                    .WhereRestrictionOn(a => a.QuestionViewGuidString)
                    .IsIn(questionViewGuidsStrings);
        if (!excludeSolutionViews)
            return result.List<Answer>();
        else
            return result.Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView).List<Answer>();
    }

    public IList<Answer> GetByQuestionViewGuid(Guid questionViewGuid)
    {
        return questionViewGuid == default(Guid)
            ? null
            : _session.QueryOver<Answer>()
                    .Where(a => a.QuestionViewGuidString == questionViewGuid.ToString())
                    .List<Answer>();
    }

    public Answer GetByQuestionViewGuid(Guid questionViewGuid, int interactionNumber)
    {
        return questionViewGuid == default(Guid)
            ? null
            : _session.QueryOver<Answer>()
                .Where(a => a.QuestionViewGuidString == questionViewGuid.ToString()
                    && a.InteractionNumber == interactionNumber )
                    .SingleOrDefault();
    }

    public Answer GetByLearningSessionStepGuid(Guid learningSessionStepGuid)
    {
        return learningSessionStepGuid == default(Guid) 
            ? null 
            : _session.QueryOver<Answer>()
                .Where(a => a.LearningSessionStepGuidString == learningSessionStepGuid.ToString())
                .List()
                .OrderByDescending(a => a.Id)
                .Last();
    }

    public override void Create(Answer answer)
    {
        _session.Save(answer);
    }

    public IList<Answer> GetAllEager(bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .Fetch(x => x.Round).Eager
            .Fetch(x => x.Player).Eager
            .Fetch(x => x.Question).Eager
            .List();
    }

    public Answer GetLastCreated(bool includingSolutionViews = false)
    {
        return Query(includingSolutionViews)
            .OrderBy(x => x.DateCreated).Desc
            .Take(1)
            .SingleOrDefault();
    }
}