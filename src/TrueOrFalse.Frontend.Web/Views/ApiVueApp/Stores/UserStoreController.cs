using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.User;

namespace VueApp;

public class UserStoreController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly SessionUser _sessionUser;
    private readonly RegisterUser _registerUser;
    private readonly PersistentLoginRepo _persistentLoginRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly GetUnreadMessageCount _getUnreadMessageCount;
    private readonly PasswordRecovery _passwordRecovery;
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly Login _login;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserStoreController(
        VueSessionUser vueSessionUser,
        SessionUser sessionUser,
        RegisterUser registerUser,
        PersistentLoginRepo persistentLoginRepo,
        UserReadingRepo userReadingRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        PasswordRecovery passwordRecovery,
        TopicControllerLogic topicControllerLogic,
        Login login,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _vueSessionUser = vueSessionUser;
        _sessionUser = sessionUser;
        _registerUser = registerUser;
        _persistentLoginRepo = persistentLoginRepo;
        _userReadingRepo = userReadingRepo;
        _getUnreadMessageCount = getUnreadMessageCount;
        _passwordRecovery = passwordRecovery;
        _topicControllerLogic = topicControllerLogic;
        _login = login;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost]
    public JsonResult Login([FromBody] LoginJson loginJson)
    {
        var isLoginErfolgreich = _login.UserLogin(loginJson);

        if (isLoginErfolgreich)
        {
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
        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor);
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
    public JsonResult Register([FromBody] RegisterJson json)
    {
        var result =  _registerUser.SetUser(json);
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.EmailInUse
            return Json(new RequestResult
           {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.UserNameInUse
           });
        }

      
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
                Type = UserType.Normal,
                ImgUrl = _sessionUser.IsLoggedIn
                    ? new UserImageSettings(_sessionUser.UserId,
                            _httpContextAccessor, 
                            _webHostEnvironment)
                        .GetUrl_20px(_sessionUser.User)
                        .Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicControllerLogic(_sessionUser, _permissionCheck, gridItemLogic).GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1)
                    .GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1)
            }

        });
    }
}



