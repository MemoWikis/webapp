using Seedworks.Lib.Persistence;
using NHibernate;


public class QuestionValuationWritingRepo : RepositoryDb<QuestionValuation>
{
    public readonly SessionUserCache SessionUserCache;

    public QuestionValuationWritingRepo(ISession session,
         SessionUserCache sessionUserCache) : base(session)
    {
        SessionUserCache = sessionUserCache;
    }
        public override void Create(IList<QuestionValuation> questionValuations)
        {
            base.Create(questionValuations);

            foreach (var questionValuation in questionValuations)
            {
                SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
            }
        }

        public override void Create(QuestionValuation questionValuation)
        {
            base.Create(questionValuation);
            SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
        }

        public override void CreateOrUpdate(QuestionValuation questionValuation)
        {
            base.CreateOrUpdate(questionValuation);
            SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
        }

        public void DeleteForQuestion(int questionId)
        {
            Session.CreateSQLQuery("DELETE FROM questionvaluation WHERE QuestionId = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();

            SessionUserCache.RemoveQuestionForAllUsers(questionId);
        }

        public override void Update(QuestionValuation questionValuation)
        {
            base.Update(questionValuation);

            SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
        }
}

