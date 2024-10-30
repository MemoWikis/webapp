using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();
    public ConcurrentDictionary<int, List<AnswerCacheItem>> Answers = new();

    public static ExtendedUserCacheItem CreateCacheItem(User user)
    {
        var sessionUserCacheItem = new ExtendedUserCacheItem();

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
                Logg.r.Error(
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
                Logg.r.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id} " +
                    $"could not be updated in {nameof(AddOrUpdateQuestionValuations)}");
        }
    }
}