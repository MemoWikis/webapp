using Seedworks.Lib.Persistence;
using static System.String;

public class UserCacheItem : IUserTinyModel, IPersistable
{
    public int ReputationPos;
    public int StartPageId;
    public int WishCountQuestions;
    public bool IsMemuchoUser => Settings.MemuchoUserId == Id;

    public virtual List<int> RecentlyUsedRelationTargetPageIds => RecentlyUsedRelationTargetPages?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToList();

    public bool AllowsSupportiveLogin { get; set; }
    public int CorrectnessProbability { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime DateCreated { get; set; }
    public IList<int> FollowerIds { get; set; }

    /// <summary>Users I follow</summary>
    public IList<int> FollowingIds { get; set; }

    public int ActivityPoints { get; set; }
    public int ActivityLevel { get; set; }

    public bool IsMember { get; set; }
    public UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string RecentlyUsedRelationTargetPages { get; set; }
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
    public int Rank { get; set; }

    private List<int> _wikiIds { get; set; } = new List<int>();
    public List<PageCacheItem?> Wikis => EntityCache.GetPages(_wikiIds);
    private List<int> _favoriteIds { get; set; } = new List<int>();
    public List<PageCacheItem?> Favorites => EntityCache.GetPages(_favoriteIds);
    public RecentPages? RecentPages { get; set; }

    public void Populate(User user)
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

        StartPageId = user.StartPageId;
        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetPages = user.RecentlyUsedRelationTargetPages;
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
        Rank = user.ReputationPos;
        DateCreated = user.DateCreated;
    }

    public void Populate(UserCacheItem user)
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

        StartPageId = user.StartPageId;
        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetPages = user.RecentlyUsedRelationTargetPages;
        WidgetHostsSpaceSeparated = user.WidgetHostsSpaceSeparated;
        CorrectnessProbability = user.CorrectnessProbability;
        TotalInOthersWishknowledge = user.TotalInOthersWishknowledge;
        FollowerIds = user.FollowerIds;
        FollowingIds = FollowingIds;
        StripeId = user.StripeId;
        EndDate = user.EndDate;
        SubscriptionStartDate = user.SubscriptionStartDate;
        IsEmailConfirmed = user.IsEmailConfirmed;
        ActivityLevel = user.ActivityLevel;
        ActivityPoints = user.ActivityPoints;
        DateCreated = user.DateCreated;
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

    public void AddWiki(int id)
    {
        if (!_wikiIds.Contains(id))
            _wikiIds.Add(id);
    }
    public void RemoveWiki(int id)
    {
        if (_wikiIds.Contains(id))
            _wikiIds.Remove(id);
    }

    public void AddFavorite(int id)
    {
        if (!_favoriteIds.Contains(id))
            _favoriteIds.Add(id);
    }
    public void RemoveFavorite(int id)
    {
        if (_favoriteIds.Contains(id))
            _favoriteIds.Remove(id);
    }
}