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
        Session.CreateSQLQuery("DELETE FROM Answer ah WHERE ah.QuestionId = :questionId")
                .SetParameter("questionId", questionId);
    }

    public IList<Answer> GetByQuestion(List<int> questionsId, int userId)
    {
        return Session.QueryOver<Answer>()
            .Where(Restrictions.In("QuestionId", questionsId))
            .And(q => q.UserId == userId)
            .List();
    }

    public IList<Answer> GetByQuestion(int questionId)
    {
        return Session.QueryOver<Answer>()
                        .Where(i => i.QuestionId == questionId)
                        .List<Answer>();
    }

    public IList<Answer> GetByQuestion(int questionId, int userId)
    {
        return Session.QueryOver<Answer>()
                        .Where(i => i.QuestionId == questionId && i.UserId == userId)
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

    public override void Create(Answer answer)
    {
        _session.Save(answer);
    }

    public IList<Answer> GetAllEager()
    {
        return Session.QueryOver<Answer>()
            .Fetch(x => x.Round).Eager
            .Fetch(x => x.Player).Eager
            .List();
    }
}