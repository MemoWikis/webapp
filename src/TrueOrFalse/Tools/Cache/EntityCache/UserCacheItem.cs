using Seedworks.Lib.Persistence;
using static System.String;

public class UserCacheItem : IUserTinyModel, IPersistable
{
    public int ReputationPos;
    public int StartTopicId;
    public int WishCountQuestions;
    public bool IsMemuchoUser => Settings.MemuchoUserId == Id;

    public virtual List<int> RecentlyUsedRelationTargetTopicIds => RecentlyUsedRelationTargetTopics?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToList();

    public bool AllowsSupportiveLogin { get; set; }
    public int CorrectnessProbability { get; set; }
    public DateTime? EndDate { get; set; }
    public IList<int> FollowerIds { get; set; }

    /// <summary>Users I follow</summary>
    public IList<int> FollowingIds { get; set; }

    public int ActivityPoints { get; set; }
    public int ActivityLevel { get; set; }

    public bool IsMember { get; set; }
    public UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string RecentlyUsedRelationTargetTopics { get; set; }
    public string StripeId { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public int TotalInOthersWishknowledge { get; set; }
    public virtual string WidgetHostsSpaceSeparated { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public string FacebookId { get; set; }
    public bool IsFacebookUser => !IsNullOrEmpty(FacebookId);
    public string GoogleId { get; set; }
    public bool IsGoogleUser => !IsNullOrEmpty(GoogleId);
    public int Reputation { get; set; }
    public int FollowerCount { get; set; }
    public bool ShowWishKnowledge { get; set; }
    public bool IsInstallationAdmin { get; set; }
    public bool IsEmailConfirmed { get; set; }

    protected void Populate(User user)
    {
        Id = user.Id;
        Name = user.Name;
        EmailAddress = user.EmailAddress;
        FacebookId = user.FacebookId;
        GoogleId = user.GoogleId;
        Reputation = user.Reputation;
        ReputationPos = user.ReputationPos;
        FollowerCount = user.FollowerCount;
        ShowWishKnowledge = user.ShowWishKnowledge;
        IsInstallationAdmin = user.IsInstallationAdmin;

        StartTopicId = user.StartTopicId;
        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetTopics = user.RecentlyUsedRelationTargetTopics;
        WidgetHostsSpaceSeparated = user.WidgetHostsSpaceSeparated;
        CorrectnessProbability = user.CorrectnessProbability;
        TotalInOthersWishknowledge = user.TotalInOthersWishknowledge;
        FollowerIds = user.Followers.Select(f => f.Follower.Id).ToList();
        FollowingIds = user.Following.Select(f => f.User.Id).ToList();
        StripeId = user.StripeId;
        EndDate = user.EndDate;
        SubscriptionStartDate = user.SubscriptionStartDate;
        IsEmailConfirmed = user.IsEmailConfirmed;
        ActivityLevel = user.ActivityLevel;
        ActivityPoints = user.ActivityPoints;
    }

    public static UserCacheItem ToCacheUser(User user)
    {
        var userCacheItem = new UserCacheItem();

        if (user != null)
        {
            userCacheItem.Populate(user);
        }

        return userCacheItem;
    }

    public static IEnumerable<UserCacheItem> ToCacheUsers(IEnumerable<User> users)
    {
        return users.Select(ToCacheUser);
    }
}