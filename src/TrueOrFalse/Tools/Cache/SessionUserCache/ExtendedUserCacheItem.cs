using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();

    public static ExtendedUserCacheItem CreateCacheItem(User user)
    {
        var sessionUserCacheItem = new ExtendedUserCacheItem();

        if (user != null)
        {
            sessionUserCacheItem.Populate(user);
        }

        return sessionUserCacheItem;
    }

    public static void AddOrUpdateQuestionValuations(ExtendedUserCacheItem extendedUser,
        QuestionValuationCacheItem questionValuationCacheItem)
    {
        extendedUser.QuestionValuations.TryGetValue(questionValuationCacheItem.Question.Id, out var valuation);
        bool result;

        if (valuation == null)
        {
            result = extendedUser.QuestionValuations.TryAdd(questionValuationCacheItem.Question.Id, questionValuationCacheItem);

            if (result == false)
                Logg.r.Error(
                    $"QuestionValuationCacheItem with Id {questionValuationCacheItem.Question.Id}" +
                    $" could not be added in {nameof(AddOrUpdateQuestionValuations)}");
        }
        else
        {
            result = extendedUser.QuestionValuations.TryUpdate(
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