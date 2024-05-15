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
}