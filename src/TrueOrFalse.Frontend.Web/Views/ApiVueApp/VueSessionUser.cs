using System.Web.Mvc;

namespace VueApp;

public class VueSessionUser
{
    public static dynamic GetCurrentUserData()
    {
        var type = UserType.Anonymous;
        if (SessionUser.IsLoggedIn)
        {
            if (SessionUser.User.IsGoogleUser)
                type = UserType.Google;
            else if (SessionUser.User.IsFacebookUser)
                type = UserType.Facebook;
            else type = UserType.Normal;

            var activityPoints = SessionUser.User.ActivityPoints;
            var activityLevel = SessionUser.User.ActivityLevel;

            return new
            {
                IsLoggedIn = SessionUser.IsLoggedIn,
                Id =  SessionUser.UserId,
                Name = SessionUser.User.Name,
                Email = SessionUser.User.EmailAddress,
                IsAdmin = SessionUser.IsInstallationAdmin,
                PersonalWikiId = SessionUser.User.StartTopicId,
                Type = type,
                ImgUrl =  new UserImageSettings(SessionUser.UserId).GetUrl_20px(SessionUser.User).Url,
                Reputation = SessionUser.User.Reputation,
                ReputationPos = SessionUser.User.ReputationPos,
                PersonalWiki = new TopicController().GetTopicData(SessionUser.User.StartTopicId),
                ActivityPoints = new
                {
                points = activityPoints,
                level = activityLevel,
                levelUp = false,
                activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel) - activityPoints,
                activityPointsPercentageOfNextLevel = activityPoints == 0 ? 0 : 100 * activityPoints / UserLevelCalculator.GetUpperLevelBound(activityLevel)
                }
            };
        }

        var userLevel = UserLevelCalculator.GetLevel(SessionUser.GetTotalActivityPoints());
        return new
        {
            IsLoggedIn = false,
            Id =  -1,
            Name = "",
            IsAdmin = false,
            PersonalWikiId = RootCategory.RootCategoryId,
            Type = type,
            ImgUrl = "",
            Reputation =  0,
            ReputationPos =0,
            PersonalWiki = new TopicController().GetTopicData(RootCategory.RootCategoryId),
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