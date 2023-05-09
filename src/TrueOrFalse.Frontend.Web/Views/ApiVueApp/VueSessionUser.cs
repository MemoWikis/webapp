using System;
using Newtonsoft.Json;
using TrueOrFalse.Stripe;

namespace VueApp;

public class VueSessionUser
{
    public static dynamic GetCurrentUserData()
    {
        var type = UserType.Anonymous;
        if (SessionUser.IsLoggedIn)
        {
            if (SessionUser.User.IsGoogleUser)
            {
                type = UserType.Google;
            }
            else if (SessionUser.User.IsFacebookUser)
            {
                type = UserType.Facebook;
            }
            else
            {
                type = UserType.Normal;
            }

            var activityPoints = SessionUser.User.ActivityPoints;
            var activityLevel = SessionUser.User.ActivityLevel;
            var subscriptionDate = SessionUser.User.SubscriptionDuration;
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };
            var json = JsonConvert.SerializeObject(DateTime.Now, settings);
            return new
            {
                SessionUser.IsLoggedIn,
                Id = SessionUser.UserId,
                SessionUser.User.Name,
                Email = SessionUser.User.EmailAddress,
                IsAdmin = SessionUser.IsInstallationAdmin,
                PersonalWikiId = SessionUser.User.StartTopicId,
                Type = type,
                ImgUrl = new UserImageSettings(SessionUser.UserId).GetUrl_50px(SessionUser.User).Url,
                SessionUser.User.Reputation,
                SessionUser.User.ReputationPos,
                PersonalWiki = new TopicControllerLogic().GetTopicData(SessionUser.User.StartTopicId),
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
                UnreadMessagesCount = Sl.Resolve<GetUnreadMessageCount>().Run(SessionUser.UserId),
                SubscriptionType = SessionUser.User.SubscriptionDuration > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                SubscriptionDuration = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate = SessionUser.User.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IsSubscriptionCanceled = subscriptionDate is
                {
                    Year: < 9999
                }
            };
        }

        var userLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());

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
            PersonalWiki = new TopicControllerLogic().GetTopicData(RootCategory.RootCategoryId),
            ActivityPoints = new
            {
                points = SessionUser.GetTotalActivityPoints(),
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