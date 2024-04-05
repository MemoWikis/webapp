using System;
using Microsoft.AspNetCore.Http;

namespace VueApp;

public class VueSessionUser(
    SessionUser _sessionUser,
    GetUnreadMessageCount _getUnreadMessageCount,
    IHttpContextAccessor _httpContextAccessor,
    UserReadingRepo _userReadingRepo,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    CategoryViewRepo _categoryViewRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo)
    : IRegisterAsInstancePerLifetime
{
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
                ImgUrl = new UserImageSettings(_sessionUser.UserId, _httpContextAccessor)
                    .GetUrl_50px_square(_sessionUser.User)
                    .Url,
                user.Reputation,
                user.ReputationPos,
                PersonalWiki = new TopicDataManager(_sessionUser,
                    _permissionCheck,
                    _knowledgeSummaryLoader,
                    _categoryViewRepo,
                    _imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo).GetTopicData(user.StartTopicId),
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
                UnreadMessagesCount = _getUnreadMessageCount.Run(_sessionUser.UserId),
                SubscriptionType = user.EndDate > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                HasStripeCustomerId = !string.IsNullOrEmpty(user.StripeId),
                EndDate = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate = user.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IsSubscriptionCanceled = subscriptionDate is
                {
                    Year: < 9999
                },
                IsEmailConfirmed = _sessionUser.IsLoggedIn && _sessionUser.User.IsEmailConfirmed
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
            PersonalWiki = new TopicDataManager(_sessionUser,
                    _permissionCheck,
                    _knowledgeSummaryLoader,
                    _categoryViewRepo,
                    _imageMetaDataReadingRepo,
                    _httpContextAccessor,
                    _questionReadingRepo)
                .GetTopicData(RootCategory.RootCategoryId),
            ActivityPoints = new
            {
                points = _sessionUser.GetTotalActivityPoints(),
                level = userLevel,
                levelUp = false,
                activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(userLevel)
            }
        };
    }

    public void Test()
    {
        var user = _userReadingRepo.GetById(445);
    }
}