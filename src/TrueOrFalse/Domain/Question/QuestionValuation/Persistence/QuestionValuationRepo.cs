using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class QuestionValuationRepo : RepositoryDb<QuestionValuation> 
{
    private readonly SearchIndexQuestion _searchIndexQuestion;
    private readonly QuestionRepo _questionRepo;

    public QuestionValuationRepo(
        ISession session, 
        SearchIndexQuestion searchIndexQuestion,
        QuestionRepo questionRepo) : base(session)
    {
        _searchIndexQuestion = searchIndexQuestion;
        _questionRepo = questionRepo;
    }

    public QuestionValuation GetBy(int questionId, int userId) => 
        _session.QueryOver<QuestionValuation>()
            .Where(q => 
                q.User.Id == userId && 
                q.Question.Id == questionId)
            .SingleOrDefault();

    public IList<QuestionValuation> GetActiveInWishknowledge(int questionId) => 
        _session.QueryOver<QuestionValuation>()
            .Where(q => 
                q.Question.Id == questionId &&
                q.RelevancePersonal > -1)
            .List<QuestionValuation>();

    public IList<QuestionValuation> GetByQuestionIds(IEnumerable<int> questionIds, int userId)
    {
        return
            _session.QueryOver<QuestionValuation>()
                    .WhereRestrictionOn(x => x.Question.Id).IsIn(questionIds.ToArray())
                    .And(x => x.User.Id == userId)
                    .List<QuestionValuation>();        
    }

    public IList<QuestionValuation> GetByUser(User user, bool onlyActiveKnowledge = true) => 
        GetByUser(user.Id, onlyActiveKnowledge);

    public IList<QuestionValuation> GetByUserFromCache(int userId, bool onlyActiveKnowledge = true)
    {
        var cacheItem = UserValuationCache.GetItem(userId);
        return cacheItem.QuestionValuations.Values.ToList();
    }

    public IList<QuestionValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q => q.User.Id == userId);

        if (onlyActiveKnowledge)
            query.And(q => q.RelevancePersonal > -1);

        return query.List<QuestionValuation>();
    }

    public IList<QuestionValuation> GetActiveInWishknowledge(IList<int> questionIds, int userId)
    {
        if(!questionIds.Any())
            return new List<QuestionValuation>();

        return _session.QueryOver<QuestionValuation>()
            .Where(qv => qv.User.Id == userId)
            .AndRestrictionOn(qv => qv.Question.Id).IsIn(questionIds.ToArray())
            .List<QuestionValuation>();
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionvaluation WHERE QuestionId = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public override void Create(IList<QuestionValuation> questionValuations)
    {
        base.Create(questionValuations);
        _searchIndexQuestion.Update(_questionRepo.GetByIds(questionValuations.QuestionIds().ToArray()));

        foreach (var questionValuation in questionValuations)
        {
            UserValuationCache.AddOrUpdate(questionValuation);
        }
    }

    public override void Create(QuestionValuation questionValuation)
    {
        base.Create(questionValuation);
        _searchIndexQuestion.Update(_questionRepo.GetById(questionValuation.Question.Id));

        UserValuationCache.AddOrUpdate(questionValuation);
    }

    public void CreateInCache(QuestionValuation questionValuation)
    {
        UserValuationCache.AddOrUpdate(questionValuation);
    }

    public override void CreateOrUpdate(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
        _searchIndexQuestion.Update(_questionRepo.GetById(questionValuation.Question.Id));

        UserValuationCache.AddOrUpdate(questionValuation);
    }

    public void CreateOrUpdateInCache(QuestionValuation questionValuation)
    {
        UserValuationCache.AddOrUpdate(questionValuation);
    }

    public override void Update(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);
        _searchIndexQuestion.Update(_questionRepo.GetById(questionValuation.Question.Id));

        UserValuationCache.AddOrUpdate(questionValuation);
    }

    public void UpdateInCache(QuestionValuation questionValuation)
    {
        UserValuationCache.AddOrUpdate(questionValuation);
    }
}