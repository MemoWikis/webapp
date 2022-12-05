using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static System.String;

public class SessionUserCacheItem
{
    public int UserId;
    public string Name;
    public string EmailAddress;
    public bool IsInstallationAdmin;

    public string FacebookId;
    public bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public string GoogleId;
    public bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public virtual bool IsMemuchoUser => Settings.MemuchoUserId == UserId;
    public int Reputation;
    public int ReputationPos;
    public int FollowerCount;
    public bool ShowWishKnowledge;
    public IList<Membership> MembershipPeriods { get; set; }
    public int ActivityPoints;
    public int ActivityLevel;

    public UserCacheItem User;

    public bool IsMember()
    {
        if (MembershipPeriods.Count == 0)
            return false;

        return MembershipPeriods.Any(x => x.IsActive(DateTime.Now));
    }

    public int StartTopicId;

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
        };
    }

    public ConcurrentDictionary<int, CategoryValuation> CategoryValuations;
    public ConcurrentDictionary<int, QuestionValuationCacheItem> QuestionValuations;
}