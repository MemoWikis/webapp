using System;

namespace VueApp;

public class VueSessionUser : IRegisterAsInstancePerLifetime
{
    private readonly SessionUser _sessionUser;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryValuationRepo _categoryValuationRepo;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryViewRepo _categoryViewRepo;
    private readonly ImageMetaDataRepo _imageMetaDataRepo;

    public VueSessionUser(SessionUser sessionUser,
        PermissionCheck permissionCheck,
        CategoryValuationRepo categoryValuationRepo,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo,
        ImageMetaDataRepo imageMetaDataRepo)
    {
        _sessionUser = sessionUser;
        _permissionCheck = permissionCheck;
        _categoryValuationRepo = categoryValuationRepo;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryViewRepo = categoryViewRepo;
        _imageMetaDataRepo = imageMetaDataRepo;
    }

    public dynamic GetCurrentUserData()
    {
        var type = UserType.Anonymous;
        var user = _sessionUser.User;

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

            return new
            {
                _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                user.Name,
                Email = user.EmailAddress,
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = user.StartTopicId,
                Type = type,
                ImgUrl = new UserImageSettings(_sessionUser.UserId).GetUrl_50px(_sessionUser.User).Url,
                user.Reputation,
                user.ReputationPos,
                PersonalWiki = new TopicControllerLogic(_sessionUser, _permissionCheck, _knowledgeSummaryLoader, _categoryValuationRepo, _categoryViewRepo,_imageMetaDataRepo).GetTopicData(user.StartTopicId),
                ActivityPoints = new
                {
                    points = activityPoints,
                    level = activityLevel,
                    levelUp = false,
                    activityPointsTillNextLevel =
                        UserLevelCalculator.GetUpperLevelBound(activityLevel) - activityPoints,
                    activityPointsPercentageOfNextLevel = activityPoints == 0
                        ? 0
                        : 100 * activityPoints / UserLevelCalculator.GetUpperLevelBound(activityLevel)
                },
                UnreadMessagesCount = Sl.Resolve<GetUnreadMessageCount>().Run(_sessionUser.UserId),
                SubscriptionType = user.EndDate > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                EndDate = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate = user.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IsSubscriptionCanceled = subscriptionDate is
                {
                    Year: < 9999
                }
            };
        }

        var userLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());

        return new
        {
            IsLoggedIn = false,
            Id = -1,
            Name = "",
            IsAdmin = false,
            PersonalWikiId = RootCategory.RootCategoryId,
            Type = type,
            ImgUrl = "",
            Reputation = 0,
            ReputationPos = 0,
            PersonalWiki = new TopicControllerLogic(_sessionUser, _permissionCheck, _knowledgeSummaryLoader, _categoryValuationRepo, _categoryViewRepo, _imageMetaDataRepo).GetTopicData(RootCategory.RootCategoryId),
            ActivityPoints = new
            {
                points = _sessionUser.GetTotalActivityPoints(),
                level = userLevel,
                levelUp = false,
                activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(userLevel)
            }
        };
    }

    private enum UserType
    {
        Normal,
        Google,
        Facebook,
        Anonymous
    }
}