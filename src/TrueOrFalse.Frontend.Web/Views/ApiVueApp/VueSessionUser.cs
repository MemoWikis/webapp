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
        }

        return new
        {
            IsLoggedIn = SessionUser.IsLoggedIn,
            Id = SessionUser.IsLoggedIn ? SessionUser.UserId : -1,
            Name = SessionUser.IsLoggedIn ? SessionUser.User.Name : "",
            IsAdmin = SessionUser.IsInstallationAdmin,
            PersonalWikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : RootCategory.RootCategoryId,
            Type = type,
            ImgUrl = SessionUser.IsLoggedIn
                ? new UserImageSettings(SessionUser.UserId).GetUrl_20px(SessionUser.User).Url
                : "",
            Reputation = SessionUser.IsLoggedIn ? SessionUser.User.Reputation : 0,
            ReputationPos = SessionUser.IsLoggedIn ? SessionUser.User.ReputationPos : 0,
            PersonalWiki = new TopicController().GetTopicData(SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : RootCategory.RootCategoryId)
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