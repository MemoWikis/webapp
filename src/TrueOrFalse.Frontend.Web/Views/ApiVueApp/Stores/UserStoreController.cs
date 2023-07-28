using System.Web.Mvc;
using Quartz;
using Quartz.Impl;

namespace VueApp;

public class UserStoreController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly SessionUser _sessionUser;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly ActivityPointsRepo _activityPointsRepo;
    private readonly RegisterUser _registerUser;
    private readonly CategoryRepository _categoryRepository;
    private readonly PersistentLoginRepo _persistentLoginRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly GetUnreadMessageCount _getUnreadMessageCount;
    private readonly PasswordRecovery _passwordRecovery;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly TopicControllerLogic _topicControllerLogic;

    public UserStoreController(
        VueSessionUser vueSessionUser,
        SessionUser sessionUser,
        CredentialsAreValid credentialsAreValid,
        ActivityPointsRepo activityPointsRepo,
        RegisterUser registerUser,
        CategoryRepository categoryRepository,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        PasswordRecovery passwordRecovery,
        UserWritingRepo userWritingRepo,
        TopicControllerLogic topicControllerLogic)
    {
        _vueSessionUser = vueSessionUser;
        _sessionUser = sessionUser;
        _credentialsAreValid = credentialsAreValid;
        _activityPointsRepo = activityPointsRepo;
        _registerUser = registerUser;
        _categoryRepository = categoryRepository;
        _persistentLoginRepo = persistentLoginRepo;
        _userReadingRepo = userReadingRepo;
        _getUnreadMessageCount = getUnreadMessageCount;
        _passwordRecovery = passwordRecovery;
        _userWritingRepo = userWritingRepo;
        _topicControllerLogic = topicControllerLogic;
    }

    [HttpPost]
    public JsonResult Login(LoginJson loginJson)
    {
        var credentialsAreValid = _credentialsAreValid;

        if (credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
        {

            if (loginJson.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id, _persistentLoginRepo);
            }

            _sessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser(_sessionUser,_activityPointsRepo);
            _userWritingRepo.UpdateActivityPointsData();

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
        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo);
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
        return _getUnreadMessageCount.Run(_sessionUser.UserId);
    }

    [HttpPost]
    public JsonResult ResetPassword(string email)
    {
        var result = _passwordRecovery.RunForNuxt(email);
        //Don't reveal if email exists 
        return Json(new RequestResult { success = result.Success || result.EmailDoesNotExist });
    } 

    [HttpPost]
    public JsonResult Register(RegisterJson json)
    {
        if (!IsEmailAddressAvailable.Yes(json.Email, _userReadingRepo))
            return Json(new
            {
                Data = new
                {
                    Success = false,
                    Message = "emailInUse",
                }
            });

        if (!global::IsUserNameAvailable.Yes(json.Name, _userReadingRepo))
            return Json(new
            {
                Data = new
                {
                    Success = false,
                    Message = "userNameInUse",
                }
            });

        var user = SetUser(json);

        _registerUser.Run(user);
        ISchedulerFactory schedFact = new StdSchedulerFactory();
        var x = schedFact.AllSchedulers;

        _sessionUser.Login(user);

        var category = PersonalTopic.GetPersonalCategory(user);
        category.Visibility = CategoryVisibility.Owner;
        _categoryRepository.Create(category);
        user.StartTopicId = category.Id;

        _userWritingRepo.Update(user);

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
                Name = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : "",
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1,
                Type = type,
                ImgUrl = _sessionUser.IsLoggedIn
                    ? new UserImageSettings(_sessionUser.UserId).GetUrl_20px(_sessionUser.User).Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = _topicControllerLogic.GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1)
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