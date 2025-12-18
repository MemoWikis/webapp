using static System.String;

public class UserCacheItem : IUserTinyModel, IPersistable
{
    public int ReputationPos;
    public int WishCountQuestions;
    public bool IsMemoWikisUser => Settings.MemoWikisUserId == Id;

    public virtual List<int> RecentlyUsedRelationTargetPageIds => RecentlyUsedRelationTargetPages?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToList();

    public bool AllowsSupportiveLogin { get; set; }
    public int CorrectnessProbability { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime DateCreated { get; set; }
    public int ActivityPoints { get; set; }
    public int ActivityLevel { get; set; }

    public bool IsMember { get; set; }
    public UserSettingNotificationInterval KnowledgeReportInterval { get; set; }
    public virtual string RecentlyUsedRelationTargetPages { get; set; }
    public string StripeId { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public int TotalInOthersWishKnowledge { get; set; }
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
    public string? AboutMeText { get; set; }

    public List<int> FavoriteIds { get; set; } = new List<int>();
    public List<PageCacheItem> Favorites => EntityCache.GetPages(FavoriteIds);
    public RecentPages? RecentPages { get; set; }
    public List<int> SharedPageIds { get; set; } = new List<int>();
    public List<int> VisibleSharedPageIds { get; set; } = new List<int>();
    public List<PageCacheItem?> SharedPages => EntityCache.GetPages(VisibleSharedPageIds);
    public MonthlyTokenUsage? MonthlyTokenUsage { get; set; }
    public virtual string UiLanguage { get; set; } = "en";
    public virtual List<Language> ContentLanguages { get; set; } = new List<Language>();
    
    /// <summary>
    /// Remaining tokens from subscription for current period
    /// </summary>
    public int SubscriptionTokensBalance { get; set; } = 100000;
    
    /// <summary>
    /// Purchased tokens that don't expire
    /// </summary>
    public int PaidTokensBalance { get; set; } = 0;

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

        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetPages = user.RecentlyUsedRelationTargetPages;
        WidgetHostsSpaceSeparated = user.WidgetHostsSpaceSeparated;
        CorrectnessProbability = user.CorrectnessProbability;
        TotalInOthersWishKnowledge = user.TotalInOthersWishKnowledge;
        StripeId = user.StripeId;
        EndDate = user.EndDate;
        SubscriptionStartDate = user.SubscriptionStartDate;
        IsEmailConfirmed = user.IsEmailConfirmed;
        ActivityLevel = user.ActivityLevel;
        ActivityPoints = user.ActivityPoints;
        Rank = user.ReputationPos;
        DateCreated = user.DateCreated;
        UiLanguage = user.UiLanguage;
        AboutMeText = user.AboutMeText;
        SubscriptionTokensBalance = user.SubscriptionTokensBalance;
        PaidTokensBalance = user.PaidTokensBalance;

        if (!String.IsNullOrEmpty(user.FavoriteIds))
            FavoriteIds = user.FavoriteIds.Split(',').Select(int.Parse).ToList();
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

        WishCountQuestions = user.WishCountQuestions;
        AllowsSupportiveLogin = user.AllowsSupportiveLogin;
        KnowledgeReportInterval = user.KnowledgeReportInterval;
        RecentlyUsedRelationTargetPages = user.RecentlyUsedRelationTargetPages;
        WidgetHostsSpaceSeparated = user.WidgetHostsSpaceSeparated;
        CorrectnessProbability = user.CorrectnessProbability;
        TotalInOthersWishKnowledge = user.TotalInOthersWishKnowledge;
        StripeId = user.StripeId;
        EndDate = user.EndDate;
        SubscriptionStartDate = user.SubscriptionStartDate;
        IsEmailConfirmed = user.IsEmailConfirmed;
        ActivityLevel = user.ActivityLevel;
        ActivityPoints = user.ActivityPoints;
        DateCreated = user.DateCreated;

        Rank = user.ReputationPos;

        FavoriteIds = user.FavoriteIds;

        UiLanguage = user.UiLanguage;
        AboutMeText = user.AboutMeText;
        SubscriptionTokensBalance = user.SubscriptionTokensBalance;
        PaidTokensBalance = user.PaidTokensBalance;
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

    public void AddFavorite(int id)
    {
        if (!FavoriteIds.Contains(id))
            FavoriteIds.Add(id);
    }

    public void RemoveFavorite(int id)
    {
        if (FavoriteIds.Contains(id))
            FavoriteIds.Remove(id);
    }


    private void RemoveDeletedFavoriteIds()
    {
        FavoriteIds = FavoriteIds.Where(id => EntityCache.GetPage(id) != null).ToList();
    }

    public int FirstWikiId => FirstWiki().Id;
    public PageCacheItem FirstWiki() => EntityCache.GetWikisByUserId(userId: Id).FirstOrDefault();
    public List<PageCacheItem> GetWikis() => EntityCache.GetWikisByUserId(userId: Id);
    public List<PageCacheItem> GetPublicWikis() => EntityCache.GetWikisByUserId(userId: Id).Where(wiki => wiki.IsPublic).ToList();

    public List<PageCacheItem> GetFavorites()
    {
        RemoveDeletedFavoriteIds();

        return Favorites.Any()
            ? Favorites
            : [];
    }


    public void PreserveContentLanguages()
    {
        var userCacheItem = EntityCache.GetUserByIdNullable(Id);
        if (userCacheItem != null)
            ContentLanguages = userCacheItem.ContentLanguages;
    }

    public void PopulateSharedPages()
    {
        var shares = EntityCache.GetSharesByUserId(Id);
        SharedPageIds = shares
            .Select(share => share.PageId)
            .Distinct()
            .ToList();

        VisibleSharedPageIds = shares
            .Where(share => share.Permission != SharePermission.RestrictAccess)
            .Select(share => share.PageId)
            .Distinct()
            .ToList();
    }

}