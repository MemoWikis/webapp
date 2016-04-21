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
        Session.CreateSQLQuery("DELETE FROM Answer ah WHERE ah.Question.Id = :questionId")
                .SetParameter("questionId", questionId);
    }

    public IList<Answer> GetByQuestion(List<int> questionsId, int userId)
    {
        return Session.QueryOver<Answer>()
            .Where(Restrictions.In("Question.Id", questionsId))
            .And(q => q.UserId == userId)
            .List();
    }

    public IList<Answer> GetByQuestion(int questionId)
    {
        return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId)
            .List<Answer>();
    }

    public IList<Answer> GetByQuestion(int questionId, int userId)
    {
        return Session.QueryOver<Answer>()
            .Where(i => i.Question.Id == questionId && i.UserId == userId)
            .List<Answer>();
    }

    public IList<Answer> GetByFeatures(AnswerFeature answerFeature, QuestionFeature questionFeature)
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

        return query.List<Answer>();
    }

    public IList<Answer> GetByCategories(int categoryId)
    {
        string query = @"
            SELECT ah.Id FROM answer ah
            LEFT JOIN question q
            ON q.Id = ah.QuestionId
            LEFT JOIN categories_to_questions cq
            ON cq.Question_id = q.Id
            WHERE cq.Category_id = " + categoryId;

        var ids = Session.CreateSQLQuery(query).List<int>();
        return GetByIds(ids.ToArray());
    }

    public IList<Answer> GetByUser(int userId)
    {
        return Session.QueryOver<Answer>()
            .Where(i => i.UserId == userId)
            .List<Answer>();
    }

    public IList<Answer> GetByUser(int userId, int amount)
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
        return GetByIds(ids.ToArray()).OrderByDescending(a => a.DateCreated).ToList();
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

    public Answer GetLastCreated()
    {
        return Session.QueryOver<Answer>()
            .OrderBy(x => x.DateCreated).Desc
            .Take(1)
            .SingleOrDefault();
    }
}