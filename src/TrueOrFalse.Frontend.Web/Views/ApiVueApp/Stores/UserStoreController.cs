using System.Linq;
using System.Web.Mvc;
using Quartz;
using Quartz.Impl;

namespace VueApp;

public class UserStoreController : BaseController
{
    [HttpPost]
    public JsonResult Login(LoginJson loginJson)
    {
        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
        {

            if (loginJson.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            SessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser();
            Sl.UserRepo.UpdateActivityPointsData();

            return Json(new
            {
                Success = true,
                Message = "",
                CurrentUser = VueSessionUser.GetCurrentUserData()
            });
        }

        return Json(new
        {
            Success = false,
            Message = "Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort."
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run();
        SessionUser.Logout();

        return Json(new
        {
            Success = !SessionUser.IsLoggedIn,
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return Sl.Resolve<GetUnreadMessageCount>().Run(SessionUser.UserId);
    }

    [HttpPost]
    public JsonResult ResetPassword(string email)
    {
        var result = Sl.Resolve<PasswordRecovery>().Run(email);
        //Don't reveal if email exists 
        return Json(new RequestResult { success = result.Success || result.EmailDoesNotExist });
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
            return Json(new
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
                PersonalWiki = new TopicControllerLogic().GetTopicData(SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1)
            }
        });
    }

    private static User SetUser(RegisterJson json)
    {
        var user = new User();
        user.EmailAddress = json.Email.TrimAndReplaceWhitespacesWithSingleSpace();
        user.Name = json.Name.TrimAndReplaceWhitespacesWithSingleSpace();
        SetUserPassword.Run(json.Password.Trim(), user);
        return user;
    }

    private enum UserType
    {
        Normal,
        Google,
        Facebook,
        Anonymous
    }
}

public class RegisterJson
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}


public class StateModel
{
    public bool IsLoggedIn { get; set; }
    public int? UserId { get; set; } = null;

}

public class LoginJson
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool PersistentLogin { get; set; }
}