using System.Collections.Concurrent;

public class SessionUserCacheItem : UserCacheItem
{
    public bool IsInstallationAdmin;

    public int ActivityPoints;
    public int ActivityLevel;

    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations = new();
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations = new();

    public static SessionUserCacheItem CreateCacheItem(User user)
    {
        var userCacheItem = new SessionUserCacheItem();

        if (user != null)
        {
            userCacheItem.AssignValues(user);
        }

        return userCacheItem;
    }

    public new void AssignValues(User user)
    {
        base.AssignValues(user);
        IsInstallationAdmin = user.IsInstallationAdmin;

        ActivityPoints = user.ActivityPoints;
        ActivityLevel = user.ActivityLevel;
    }
}