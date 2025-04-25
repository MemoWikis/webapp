public class FrontEndUserData(
    SessionUser _sessionUser,
    GetUnreadMessageCount _getUnreadMessageCount,
    IHttpContextAccessor _httpContextAccessor,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
    public readonly record struct CurrentUserData(
        bool IsLoggedIn,
        int Id,
        string Name,
        string Email,
        bool IsAdmin,
        int PersonalWikiId,
        UserType Type,
        string ImgUrl,
        int Reputation,
        int ReputationPos,
        PageDataManager.PageDataResult PersonalWiki,
        ActivityPoints ActivityPoints,
        int UnreadMessagesCount,
        SubscriptionType SubscriptionType,
        bool HasStripeCustomerId,
        string EndDate,
        string SubscriptionStartDate,
        bool IsSubscriptionCanceled,
        bool IsEmailConfirmed,
        string CollaborationToken,
        string UiLanguage);

    public record struct ActivityPoints(
        int Points,
        int Level,
        bool LevelUp,
        int ActivityPointsTillNextLevel,
        int ActivityPointsPercentageOfNextLevel);

    public CurrentUserData Get()
    {
        var type = UserType.Anonymous;
        var user = GetSessionUserUser();

        if (_sessionUser.IsLoggedIn)
        {
            if (user.IsGoogleUser)
                type = UserType.Google;
            else if (user.IsFacebookUser)
                type = UserType.Facebook;
            else
                type = UserType.Normal;

            var activityPoints = user.ActivityPoints;
            var activityLevel = user.ActivityLevel;
            var subscriptionDate = user.EndDate;

            return new CurrentUserData
            {
                IsLoggedIn = _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                Name = user.Name,
                Email = user.EmailAddress,
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = user.StartPageId,
                Type = type,
                ImgUrl = new UserImageSettings(_sessionUser.UserId, _httpContextAccessor)
                    .GetUrl_50px_square(_sessionUser.User)
                    .Url,
                Reputation = user.Reputation,
                ReputationPos = user.ReputationPos,
                PersonalWiki = new PageDataManager(_sessionUser,
                    _permissionCheck,
                    _knowledgeSummaryLoader,
                    _imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo).GetPageData(user.StartPageId),
                ActivityPoints = new ActivityPoints
                {
                    Points = activityPoints,
                    Level = activityLevel,
                    LevelUp = false,
                    ActivityPointsTillNextLevel =
                        UserLevelCalculator.GetUpperLevelBound(activityLevel) - activityPoints,
                    ActivityPointsPercentageOfNextLevel = activityPoints == 0
                        ? 0
                        : 100 * activityPoints /
                          UserLevelCalculator.GetUpperLevelBound(activityLevel)
                },
                UnreadMessagesCount = _getUnreadMessageCount.Run(_sessionUser.UserId),
                SubscriptionType = user.EndDate > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                HasStripeCustomerId = !string.IsNullOrEmpty(user.StripeId),
                EndDate = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate =
                    user.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IsSubscriptionCanceled = subscriptionDate is
                {
                    Year: < 9999
                },
                IsEmailConfirmed = _sessionUser.IsLoggedIn && _sessionUser.User.IsEmailConfirmed,
                CollaborationToken = new CollaborationToken().Get(_sessionUser.UserId),
                UiLanguage = _sessionUser.User.UiLanguage
            };
        }

        var userLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());
        return new CurrentUserData
        {
            IsLoggedIn = false,
            Id = -1,
            Name = "",
            IsAdmin = false,
            PersonalWikiId = FeaturedPage.RootPageId,
            Type = type,
            ImgUrl = "",
            Reputation = 0,
            ReputationPos = 0,
            PersonalWiki = new PageDataManager(_sessionUser,
                    _permissionCheck,
                    _knowledgeSummaryLoader,
                    _imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetPageData(FeaturedPage.RootPageId),
            ActivityPoints = new ActivityPoints
            {
                Points = _sessionUser.GetTotalActivityPoints(),
                Level = userLevel,
                LevelUp = false,
                ActivityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(userLevel)
            }
        };
    }

    private ExtendedUserCacheItem GetSessionUserUser()
    {
        if (_sessionUser.IsLoggedIn == false)
            return null;

        return _sessionUser.User;
    }
}