

namespace TrueOrFalse.Domain.Question.QuestionValuation
{
    public class QuestionValuationCache
    {
        private readonly SessionUserCache _sessionUserCache;

        public QuestionValuationCache(SessionUserCache sessionUserCache)
        {
            _sessionUserCache = sessionUserCache;
        }

        public QuestionValuationCacheItem GetByFromCache(int questionId, int userId)
        {
            return _sessionUserCache.GetItem(userId)?.QuestionValuations
                .Where(v => v.Value.Question.Id == questionId)
                .Select(v => v.Value)
                .FirstOrDefault();
        }

        public IList<QuestionValuationCacheItem> GetByQuestionsFromCache(IList<QuestionCacheItem> questions)
        {
            var questionValuations = _sessionUserCache.GetAllCacheItems().Select(c => c.QuestionValuations.Values)
                .SelectMany(l => l);

            return questionValuations.Where(v => questions.GetIds().Contains(v.Question.Id)).ToList();
        }


        public IList<QuestionValuationCacheItem> GetByUserFromCache(int userId, bool onlyActiveKnowledge = true)
        {
            var cacheItem = _sessionUserCache.GetItem(userId);
            return cacheItem.QuestionValuations.Values.ToList();
        }
    }
}
