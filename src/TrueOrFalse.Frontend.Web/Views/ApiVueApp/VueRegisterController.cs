using System.Web.Mvc;
using Quartz;
using Quartz.Impl;
using TrueOrFalse.Frontend.Web.Code;
using VueApp;

public class VueRegisterController : BaseController
{
    public class RegisterJson
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    private static User SetUser(RegisterJson json)
    {
        var user = new User();
        user.EmailAddress = json.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = json.Name.TrimAndReplaceWhitespacesWithSingleSpace();
        SetUserPassword.Run(json.Password.Trim(), user);
        return user;
    }

    [HttpPost]
    public JsonResult Register(RegisterJson json)
    {
        if (!IsEmailAddressAvailable.Yes(json.Email))
            return Json(new
            {
                Data = new
                {
                    Success = false,
                    Message = "emailInUse",
                }
            });

        if (!global::IsUserNameAvailable.Yes(json.Name))
            return Json( new
                {
                    Data = new
                    {
                        Success = false,
                        Message = "userNameInUse",
                    }
                });

        var user = SetUser(json);

        RegisterUser.Run(user);
        ISchedulerFactory schedFact = new StdSchedulerFactory();
        var x = schedFact.AllSchedulers;

        SessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        Sl.CategoryRepo.Create(category);
        user.StartTopicId = category.Id;

        Sl.UserRepo.Update(user);

        var type = UserType.Anonymous;
        if (SessionUser.IsLoggedIn)
        {
            if (SessionUser.User.IsGoogleUser)
                type = UserType.Google;
            else if (SessionUser.User.IsFacebookUser)
                type = UserType.Facebook;
            else type = UserType.Normal;
        }

        return Json(new
        {
            Success = true,
            Message = "",
            CurrentUser = new
            {
                IsLoggedIn = SessionUser.IsLoggedIn,
                Id = SessionUser.UserId,
                Name = SessionUser.IsLoggedIn ? SessionUser.User.Name : "",
                IsAdmin = SessionUser.IsInstallationAdmin,
                PersonalWikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1,
                Type = type,
                ImgUrl = SessionUser.IsLoggedIn
                    ? new UserImageSettings(SessionUser.UserId).GetUrl_20px(SessionUser.User).Url
                    : "",
                Reputation = SessionUser.IsLoggedIn ? SessionUser.User.Reputation : 0,
                ReputationPos = SessionUser.IsLoggedIn ? SessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicController().GetTopicData(SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1)
            }
        });
    }

    private enum UserType
    {
        Normal,
        Google,
        Facebook,
        Anonymous
    }
}