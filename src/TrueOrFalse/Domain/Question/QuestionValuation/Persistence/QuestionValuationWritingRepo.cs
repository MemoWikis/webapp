using Seedworks.Lib.Persistence;
using NHibernate;

public class QuestionValuationWritingRepo(
    ISession _session,
    ExtendedUserCache extendedUserCache)
    : RepositoryDb<QuestionValuation>(_session)
{
    public override void Create(IList<QuestionValuation> questionValuations)
    {
        base.Create(questionValuations);

        foreach (var questionValuation in questionValuations)
            extendedUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public override void Create(QuestionValuation questionValuation)
    {
        base.Create(questionValuation);
        extendedUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public override void CreateOrUpdate(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
        extendedUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public void DeleteForQuestion(int questionId)
    {
        Session
            .CreateSQLQuery("DELETE FROM questionvaluation WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();

        extendedUserCache.RemoveQuestionForAllUsers(questionId);
    }

    public override void Update(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);
        extendedUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }
}