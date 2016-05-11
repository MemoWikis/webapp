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

    public IList<Answer> GetByQuestion(List<int> questionsId, int userId, bool includingSolutionViews = false)
    {
        if(includingSolutionViews)
            return Session.QueryOver<Answer>()
            .Where(Restrictions.In("Question.Id", questionsId))
            .And(a => a.UserId == userId)
            .List();

        return Session.QueryOver<Answer>()
            .Where(Restrictions.In("Question.Id", questionsId))
            .And(a => a.UserId == userId)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List();
    }

    public IList<Answer> GetByQuestion(int questionId, bool includingSolutionViews = false)
    {
        if (includingSolutionViews)
            return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId)
            .List<Answer>();

        return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView )
            .List<Answer>();
    }

    public IList<Answer> GetByQuestion(int questionId, int userId, bool includingSolutionViews = false)
    {
        if (includingSolutionViews)
            return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId && i.UserId == userId)
            .List<Answer>();

        return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId && i.UserId == userId)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView )
            .List<Answer>();
    }

    public IList<Answer> GetByFeatures(AnswerFeature answerFeature, QuestionFeature questionFeature, bool includingSolutionViews = false)
    {
        var query = Session.QueryOver<Answer>();

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

        if (!includingSolutionViews)
            query = query.Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView);

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
        if(includingSolutionViews)
            return Session.QueryOver<Answer>()
                .Where(i => i.UserId == userId)
                .List<Answer>();

        return Session.QueryOver<Answer>()
            .Where(i => i.UserId == userId)
            .And(a => a.AnswerredCorrectly != AnswerCorrectness.IsView)
            .List<Answer>();
    }

    public IList<Answer> GetByUser(int userId, int amount, bool includingSolutionViews = false)
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
            LIMIT "+amount;
        var ids = Session.CreateSQLQuery(query).List<int>();

        if(includingSolutionViews)
            return GetByIds(ids.ToArray()).OrderByDescending(a => a.DateCreated).ToList();

        return GetByIds(ids.ToArray()).Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView).OrderByDescending(a => a.DateCreated).ToList();
    }

    public override void Create(Answer answer)
    {
        _session.Save(answer);
    }

    public IList<Answer> GetAllEager()
    {
        return Session.QueryOver<Answer>()
            .Fetch(x => x.Round).Eager
            .Fetch(x => x.Player).Eager
            .Fetch(x => x.Question).Eager
            .List();
    }

    public Answer GetLastCreated(bool includingSolutionViews = false)
    {
        var query = Session.QueryOver<Answer>();

        if (!includingSolutionViews)
            query = query.Where(a => a.AnswerredCorrectly != AnswerCorrectness.IsView);

        return query
            .OrderBy(x => x.DateCreated).Desc
            .Take(1)
            .SingleOrDefault();
    }
}