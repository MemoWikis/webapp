using System.Web.Mvc;
using Quartz;
using Quartz.Impl;

namespace VueApp;

public class UserStoreController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly SessionUser _sessionUser;
    private readonly CredentialsAreValid _credentialsAreValid;

    public UserStoreController(VueSessionUser vueSessionUser,  SessionUser sessionUser, CredentialsAreValid credentialsAreValid)
    {
        _vueSessionUser = vueSessionUser;
        _sessionUser = sessionUser;
        _credentialsAreValid = credentialsAreValid;
    }
    [HttpPost]
    public JsonResult Login(LoginJson loginJson)
    {
        var credentialsAreValid = _credentialsAreValid;

        if (credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
        {

            if (loginJson.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            _sessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser();
            Sl.UserRepo.UpdateActivityPointsData();

            return Json(new
            {
                Success = true,
                Message = "",
                CurrentUser = _vueSessionUser.GetCurrentUserData()
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
        _sessionUser.Logout();

        return Json(new
        {
            Success = !_sessionUser.IsLoggedIn,
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return Sl.Resolve<GetUnreadMessageCount>().Run(_sessionUser.UserId);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public void ResetPassword(string email) => Sl.Resolve<PasswordRecovery>().Run(email);

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

        _sessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        Sl.CategoryRepo.Create(category);
        user.StartTopicId = category.Id;

        Sl.UserRepo.Update(user);

        var type = UserType.Anonymous;
        if (_sessionUser.IsLoggedIn)
        {
            if (_sessionUser.User.IsGoogleUser)
                type = UserType.Google;
            else if (_sessionUser.User.IsFacebookUser)
                type = UserType.Facebook;
            else type = UserType.Normal;
        }

        return Json(new
        {
            Success = true,
            Message = "",
            CurrentUser = new
            {
                IsLoggedIn = _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                Name = _sessionUser.IsLoggedIn ? SessionUserLegacy.User.Name : "",
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.StartTopicId : 1,
                Type = type,
                ImgUrl = SessionUserLegacy.IsLoggedIn
                    ? new UserImageSettings(SessionUserLegacy.UserId).GetUrl_20px(SessionUserLegacy.User).Url
                    : "",
                Reputation = SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.Reputation : 0,
                ReputationPos = SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.ReputationPos : 0,
                PersonalWiki = new TopicControllerLogic(_sessionUser).GetTopicData(SessionUserLegacy.IsLoggedIn ? SessionUserLegacy.User.StartTopicId : 1)
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