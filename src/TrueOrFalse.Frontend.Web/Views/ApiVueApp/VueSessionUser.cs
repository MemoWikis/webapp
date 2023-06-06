using System;
using System.Linq.Expressions;
using System.Web;
using Newtonsoft.Json;
using TrueOrFalse.Stripe;

namespace VueApp;

public class VueSessionUser : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext;
    private readonly SessionUser _sessionUser; 
    public VueSessionUser(HttpContext httpContext)
    {
        _httpContext = httpContext;
        if (httpContext == null)
        {
            Logg.r().Error("httpcontext is Empty VueSessionUser");
            throw new NullReferenceException("httpcontext is Empty VueSessionUser"); 
        }
        _sessionUser = new SessionUser(httpContext);
    }
    public dynamic GetCurrentUserData()
    {
        var type = UserType.Anonymous;
        if (_sessionUser.IsLoggedIn)
        {
            if (_sessionUser.User.IsGoogleUser)
            {
                type = UserType.Google;
            }
            else if (_sessionUser.User.IsFacebookUser)
            {
                type = UserType.Facebook;
            }
            else
            {
                type = UserType.Normal;
            }

            var activityPoints = _sessionUser.User.ActivityPoints;
            var activityLevel = _sessionUser.User.ActivityLevel;
            var subscriptionDate = _sessionUser.User.EndDate;
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ" };
            var json = JsonConvert.SerializeObject(DateTime.Now, settings);
            return new
            {
                _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                _sessionUser.User.Name,
                Email = _sessionUser.User.EmailAddress,
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = _sessionUser.User.StartTopicId,
                Type = type,
                ImgUrl = new UserImageSettings(_sessionUser.UserId).GetUrl_50px(_sessionUser.User).Url,
                _sessionUser.User.Reputation,
                _sessionUser.User.ReputationPos,
                PersonalWiki = new TopicControllerLogic(_sessionUser).GetTopicData(_sessionUser.User.StartTopicId),
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
                SubscriptionType = _sessionUser.User.EndDate > DateTime.Now
                    ? SubscriptionType.Plus
                    : SubscriptionType.Basic,
                EndDate = subscriptionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                SubscriptionStartDate = _sessionUser.User.SubscriptionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
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
            PersonalWiki = new TopicControllerLogic(_sessionUser).GetTopicData(RootCategory.RootCategoryId),
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