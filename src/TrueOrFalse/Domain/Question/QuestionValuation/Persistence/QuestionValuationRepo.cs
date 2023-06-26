using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

public class QuestionValuationRepo : RepositoryDb<QuestionValuation>
{
 
    private readonly QuestionRepo _questionRepo;

    public QuestionValuationRepo(
        ISession session,
        QuestionRepo questionRepo) : base(session)
    {
        _questionRepo = questionRepo;
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

    public void CreateInDatabase(QuestionValuation questionValuation)
    {
        base.Create(questionValuation);
    }

    public override void CreateOrUpdate(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
        SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public void CreateOrUpdateInCache(QuestionValuationCacheItem questionValuation)
    {
        SessionUserCache.AddOrUpdate(questionValuation);
    }

    public void CreateOrUpdateInDatabase(QuestionValuation questionValuation)
    {
        base.CreateOrUpdate(questionValuation);
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionvaluation WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();

        SessionUserCache.RemoveQuestionForAllUsers(questionId);
    }

    public IList<QuestionValuation> GetActiveInWishknowledge(IList<int> questionIds, int userId)
    {
        if (!questionIds.Any())
        {
            return new List<QuestionValuation>();
        }

        return _session.QueryOver<QuestionValuation>()
            .Where(qv => qv.User.Id == userId)
            .AndRestrictionOn(qv => qv.Question.Id).IsIn(questionIds.ToArray())
            .List<QuestionValuation>();
    }

    public IList<QuestionValuationCacheItem> GetActiveInWishknowledgeFromCache(int questionId)
    {
        return SessionUserCache.GetAllCacheItems()
            .Select(c => c.QuestionValuations.Values).SelectMany(v => v)
            .Where(v =>
                v.Question.Id == questionId &&
                v.IsInWishKnowledge)
            .ToList();
    }

    public IList<QuestionValuationCacheItem> GetActiveInWishknowledgeFromCache(IList<int> questionIds, int userId)
    {
        if (!questionIds.Any())
        {
            return new List<QuestionValuationCacheItem>();
        }

        return SessionUserCache.GetItem(userId).QuestionValuations
            .Where(v => questionIds.Contains(v.Value.Question.Id))
            .Select(c => c.Value).ToList();
    }

    public QuestionValuation GetBy(int questionId, int userId)
    {
        return _session.QueryOver<QuestionValuation>()
            .Where(q =>
                q.User.Id == userId &&
                q.Question.Id == questionId)
            .SingleOrDefault();
    }

    public QuestionValuationCacheItem GetByFromCache(int questionId, int userId)
    {
        return SessionUserCache.GetItem(userId)?.QuestionValuations
            .Where(v => v.Value.Question.Id == questionId)
            .Select(v => v.Value)
            .FirstOrDefault();
    }

    public IList<QuestionValuationCacheItem> GetByQuestionFromCache(Question question)
    {
        var questionValuations = SessionUserCache.GetAllCacheItems().Select(c => c.QuestionValuations.Values)
            .SelectMany(v => v);

        return questionValuations.Where(v => v.Question.Id == question.Id).ToList();
    }

    public IList<QuestionValuation> GetByQuestionIds(IEnumerable<int> questionIds, int userId)
    {
        return
            _session.QueryOver<QuestionValuation>()
                .WhereRestrictionOn(x => x.Question.Id).IsIn(questionIds.ToArray())
                .And(x => x.User.Id == userId)
                .List<QuestionValuation>();
    }

    public IList<QuestionValuationCacheItem> GetByQuestionsAndUserFromCache(IEnumerable<int> questionIds, int userId)
    {
        return SessionUserCache.GetItem(userId).QuestionValuations.Values
            .Where(v => questionIds.Contains(v.Question.Id))
            .ToList();
    }

    public IList<QuestionValuationCacheItem> GetByQuestionsFromCache(IList<QuestionCacheItem> questions)
    {
        var questionValuations = SessionUserCache.GetAllCacheItems().Select(c => c.QuestionValuations.Values)
            .SelectMany(l => l);

        return questionValuations.Where(v => questions.GetIds().Contains(v.Question.Id)).ToList();
    }

    public IList<QuestionValuation> GetByUser(User user, bool onlyActiveKnowledge = true)
    {
        return GetByUser(user.Id, onlyActiveKnowledge);
    }

    public IList<QuestionValuation> GetByUser(int userId, bool onlyActiveKnowledge = true)
    {
        var query = _session
            .QueryOver<QuestionValuation>()
            .Where(q => q.User.Id == userId);

        if (onlyActiveKnowledge)
        {
            query.And(q => q.RelevancePersonal > -1);
        }

        return query.List<QuestionValuation>();
    }

    public IList<QuestionValuationCacheItem> GetByUserFromCache(int userId, bool onlyActiveKnowledge = true)
    {
        var cacheItem = SessionUserCache.GetItem(userId);
        return cacheItem.QuestionValuations.Values.ToList();
    }

    public IList<QuestionValuation> GetByUserWithQuestion(int userId)
    {
        var r = _session
            .Query<QuestionValuation>().Where(qv => qv.User.Id == userId).Fetch(qv => qv.Question).ToList();

        var result = _session
            .Query<QuestionValuation>()
            .Where(qv => qv.User.Id == userId)
            .Fetch(qv => qv.Question)
            .ThenFetchMany(q => q.Categories)
            .ToList();

        return result;
    }

    public override void Update(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);

        SessionUserCache.AddOrUpdate(questionValuation.ToCacheItem());
    }

    public void UpdateInDatabase(QuestionValuation questionValuation)
    {
        base.Update(questionValuation);
    }
}