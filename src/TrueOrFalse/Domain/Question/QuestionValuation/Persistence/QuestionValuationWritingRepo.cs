using Seedworks.Lib.Persistence;
using NHibernate;

public class QuestionValuationWritingRepo(
    ISession _session,
    SessionUserCache _sessionUserCache)
    : RepositoryDb<QuestionValuation>(_session)
{
    public override void Create(IList<QuestionValuation> questionValuations)
    {
        base.Create(questionValuations);

        foreach (var questionValuation in questionValuations)
            _sessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public override void Create(QuestionValuation questionValuation)
    {
        base.Create(questionValuation);
        _sessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public override void CreateOrUpdate(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
        _sessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public void DeleteForQuestion(int questionId)
    {
        Session
            .CreateSQLQuery("DELETE FROM questionvaluation WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();

        _sessionUserCache.RemoveQuestionForAllUsers(questionId);
    }

    public override void Update(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);
        _sessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }
}