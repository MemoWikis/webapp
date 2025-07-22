public class QuestionValuationCache(LoggedInUserCache _loggedInUserCache)
{
    public QuestionValuationCacheItem GetByFromCache(int questionId, int userId)
    {
        return _loggedInUserCache.GetItem(userId)?.QuestionValuations
            .Where(v => v.Value.Question.Id == questionId)
            .Select(v => v.Value)
            .FirstOrDefault();
    }

    public IList<QuestionValuationCacheItem> GetByQuestionsFromCache(
        IList<QuestionCacheItem> questions)
    {
        var questionValuations = _loggedInUserCache.GetAllCacheItems()
            .Select(c => c.QuestionValuations.Values)
            .SelectMany(l => l);

        return questionValuations.Where(v => questions.GetIds().Contains(v.Question.Id))
            .ToList();
    }

    public IList<QuestionValuationCacheItem> GetByUserFromCache(int userId)
    {
        var cacheItem = _loggedInUserCache.GetItem(userId);

        if (cacheItem == null)
            return new List<QuestionValuationCacheItem>();

        return cacheItem.QuestionValuations.Values.ToList();
    }
}