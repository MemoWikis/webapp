using System.Collections.Concurrent;

public class ExtendedUserCacheItem : UserCacheItem
{
    public bool IsInstallationAdmin;

    public int ActivityPoints;
    public int ActivityLevel;

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

    public new void Populate(User user)
    {
        base.Populate(user);
        IsInstallationAdmin = user.IsInstallationAdmin;

        ActivityPoints = user.ActivityPoints;
        ActivityLevel = user.ActivityLevel;
    }

    public static ExtendedUserCacheItem CreateCacheItem(UserCacheItem userCacheItem)
    {
        var sessionUserCacheItem = new ExtendedUserCacheItem();

        if (userCacheItem != null)
        {
            sessionUserCacheItem.Populate(userCacheItem);
        }

        return sessionUserCacheItem;
    }

    public void Populate(UserCacheItem userCacheItem)
    {
        IsInstallationAdmin = userCacheItem.IsInstallationAdmin;

        ActivityPoints = userCacheItem.ActivityPoints;
        ActivityLevel = userCacheItem.ActivityLevel;
    }
}