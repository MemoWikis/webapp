using System.Web.Mvc;
using Quartz;
using Quartz.Impl;

namespace VueApp;

public class UserStoreController : BaseController
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly PermissionCheck _permissionCheck;
    private readonly ActivityPointsRepo _activityPointsRepo;
    private readonly RegisterUser _registerUser;
    private readonly UserRepo _userRepo;

    public UserStoreController(
        VueSessionUser vueSessionUser,
        SessionUser sessionUser,
        CredentialsAreValid credentialsAreValid,
        PermissionCheck permissionCheck,
        ActivityPointsRepo activityPointsRepo,
        RegisterUser registerUser,
        UserRepo userRepo) : base(sessionUser)
    {
        _vueSessionUser = vueSessionUser;
        _credentialsAreValid = credentialsAreValid;
        _permissionCheck = permissionCheck;
        _activityPointsRepo = activityPointsRepo;
        _registerUser = registerUser;
        _userRepo = userRepo;
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

            TransferActivityPoints.FromSessionToUser(_sessionUser,_activityPointsRepo);
            _userRepo.UpdateActivityPointsData();

            return Json(new RequestResult
            {
                success = true,
                data = _vueSessionUser.GetCurrentUserData()
            });

        }
        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.LoginFailed
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run();
        _sessionUser.Logout();

        if (!_sessionUser.IsLoggedIn) 
            return Json(new RequestResult
            {
                success = true,
            });

        return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.Default
        });
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return Sl.Resolve<GetUnreadMessageCount>().Run(_sessionUser.UserId);
    }

    [HttpPost]
    public JsonResult ResetPassword(string email)
    {
        var result = Sl.Resolve<PasswordRecovery>().RunForNuxt(email);
        //Don't reveal if email exists 
        return Json(new RequestResult { success = result.Success || result.EmailDoesNotExist });
    } 

    [HttpPost]
    public JsonResult Register(RegisterJson json)
    {
        if (!IsEmailAddressAvailable.Yes(json.Email))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.EmailInUse
            });

        if (!IsUserNameAvailable.Yes(json.Name))
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.UserNameInUse
            });

        var user = CreateUserFromJson(json);

        _registerUser.Run(user);
        _sessionUser.Login(user);
        _registerUser.CreateStartTopicAndSetToUser(user);
        _registerUser.SendWelcomeAndRegistrationEmails(user);

        var type = UserType.Anonymous;
        if (_sessionUser.IsLoggedIn)
        {
            if (_sessionUser.User.IsGoogleUser)
                type = UserType.Google;
            else if (_sessionUser.User.IsFacebookUser)
                type = UserType.Facebook;
            else type = UserType.Normal;
        }

        var gridItemLogic = new GridItemLogic(_permissionCheck, _sessionUser);
        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                IsLoggedIn = _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                Name = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : "",
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1,
                Type = type,
                ImgUrl = _sessionUser.IsLoggedIn
                    ? new UserImageSettings(_sessionUser.UserId).GetUrl_20px(_sessionUser.User).Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicControllerLogic(_sessionUser, _permissionCheck, gridItemLogic).GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1)
            }

        });
    }

    private static User CreateUserFromJson(RegisterJson json)
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

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RequestVerificationMail()
    {
        SendConfirmationEmail.Run(_sessionUser.User.Id);
        return Json(new RequestResult
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.User.VerificationMailRequestSent
        });
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