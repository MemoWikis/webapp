using System.Collections.Concurrent;

public class LoggedInUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, PageValuation> PageValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();
    public ConcurrentDictionary<int, AnswerRecord> AnswerCounter = new();

    public static LoggedInUserCacheItem CreateCacheItem(User user, PageViewRepo pageViewRepo)
    {
        var sessionUserCacheItem = new LoggedInUserCacheItem();

        if (user != null)
        {
            sessionUserCacheItem.Populate(user);
        }

        return sessionUserCacheItem;
    }

    public void AddOrUpdateQuestionValuations(QuestionValuationCacheItem questionValuationCacheItem)
    {
        QuestionValuations.TryGetValue(questionValuationCacheItem.Question.Id, out var valuation);

        if (valuation == null)
        {
            var result = QuestionValuations
                .TryAdd(questionValuationCacheItem.Question.Id, questionValuationCacheItem);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id}" +
                    $" could not be added in {nameof(AddOrUpdateQuestionValuations)}");
        }
        else
        {
            var result = QuestionValuations.TryUpdate(
                questionValuationCacheItem.Question.Id,
                questionValuationCacheItem,
                valuation);

            if (result == false)
                Log.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id} " +
                    $"could not be updated in {nameof(AddOrUpdateQuestionValuations)}");
        }
    }
}