using System;
using System.Linq.Expressions;
using System.Web;
using Newtonsoft.Json;
using TrueOrFalse.Stripe;

namespace VueApp;

public class VueSessionUser : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;

    public VueSessionUser(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }
    public dynamic GetCurrentUserData()
    {
        var type = UserType.Anonymous;
        if (SessionUserLegacy.IsLoggedIn)
        {
            if (SessionUserLegacy.User.IsGoogleUser)
            {
                type = UserType.Google;
            }
            else if (SessionUserLegacy.User.IsFacebookUser)
            {
                type = UserType.Facebook;
            }
            else
            {
                type = UserType.Normal;
            }

            var activityPoints = SessionUserLegacy.User.ActivityPoints;
            var activityLevel = SessionUserLegacy.User.ActivityLevel;
            var subscriptionDate = SessionUserLegacy.User.EndDate;
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };
            var json = JsonConvert.SerializeObject(DateTime.Now, settings);
            return new
            {
                SessionUserLegacy.IsLoggedIn,
                Id = SessionUserLegacy.UserId,
                SessionUserLegacy.User.Name,
                Email = SessionUserLegacy.User.EmailAddress,
                IsAdmin = SessionUserLegacy.IsInstallationAdmin,
                PersonalWikiId = SessionUserLegacy.User.StartTopicId,
                Type = type,
                ImgUrl = new UserImageSettings(SessionUserLegacy.UserId).GetUrl_50px(SessionUserLegacy.User).Url,
                SessionUserLegacy.User.Reputation,
                SessionUserLegacy.User.ReputationPos,
                PersonalWiki = new TopicControllerLogic(new SessionUser(_httpContext)).GetTopicData(SessionUserLegacy.User.StartTopicId),
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
                UnreadMessagesCount = Sl.Resolve<GetUnreadMessageCount>().Run(SessionUserLegacy.UserId),
                SubscriptionType = SessionUserLegacy.User.EndDate > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                EndDate = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate = SessionUserLegacy.User.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                IsSubscriptionCanceled = subscriptionDate is
                {
                    Year: < 9999
                }
            };
        }

        var userLevel = UserLevelCalculator.GetLevel(SessionUserLegacy.GetTotalActivityPoints());

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
            PersonalWiki = new TopicControllerLogic(new SessionUser(_httpContext)).GetTopicData(RootCategory.RootCategoryId),
            ActivityPoints = new
            {
                points = SessionUserLegacy.GetTotalActivityPoints(),
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