using System;
using System.Collections.Concurrent;
using System.Linq;

public class SessionUserCacheItem : UserCacheItem
{
    public int UserId;
    public bool IsInstallationAdmin;

    public int ActivityPoints;
    public int ActivityLevel;

    public UserCacheItem User;

    public static SessionUserCacheItem CreateCacheItem(User user)
    {
        return new SessionUserCacheItem
        {
            UserId = user.Id,
            Name = user.Name,
            EmailAddress = user.EmailAddress,
            IsInstallationAdmin = user.IsInstallationAdmin,

            FacebookId = user.FacebookId,
            GoogleId = user.GoogleId,
            Reputation = user.Reputation,
            ReputationPos = user.ReputationPos,
            FollowerCount = user.FollowerCount,
            ShowWishKnowledge = user.ShowWishKnowledge,

            StartTopicId = user.StartTopicId,
            ActivityPoints = user.ActivityPoints,
            ActivityLevel = user.ActivityLevel,

            CategoryValuations = new ConcurrentDictionary<int, CategoryValuation>(),
            QuestionValuations = new ConcurrentDictionary<int, QuestionValuationCacheItem>()
        };
    }

    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations;
}