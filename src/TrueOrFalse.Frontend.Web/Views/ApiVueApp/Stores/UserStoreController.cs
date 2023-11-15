using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Domain.User;

namespace VueApp;

public class UserStoreController : Controller
{
    private readonly VueSessionUser _vueSessionUser;
    private readonly SessionUser _sessionUser;
    private readonly RegisterUser _registerUser;
    private readonly PersistentLoginRepo _persistentLoginRepo;
    private readonly GetUnreadMessageCount _getUnreadMessageCount;
    private readonly PasswordRecovery _passwordRecovery;
    private readonly Login _login;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly PermissionCheck _permissionCheck;
    private readonly GridItemLogic _gridItemLogic;
    private readonly KnowledgeSummaryLoader _knowledgeSummaryLoader;
    private readonly CategoryViewRepo _categoryViewRepo;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly JobQueueRepo _jobQueueRepo;

    public UserStoreController(VueSessionUser vueSessionUser,
        SessionUser sessionUser,
        RegisterUser registerUser,
        PersistentLoginRepo persistentLoginRepo,
        GetUnreadMessageCount getUnreadMessageCount,
        PasswordRecovery passwordRecovery,
        Login login,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        PermissionCheck permissionCheck,
        GridItemLogic gridItemLogic,
        KnowledgeSummaryLoader knowledgeSummaryLoader,
        CategoryViewRepo categoryViewRepo,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IActionContextAccessor actionContextAccessor,
        UserReadingRepo userReadingRepo,
        QuestionReadingRepo questionReadingRepo,
        JobQueueRepo jobQueueRepo)
    {
        _vueSessionUser = vueSessionUser;
        _sessionUser = sessionUser;
        _registerUser = registerUser;
        _persistentLoginRepo = persistentLoginRepo;
        _getUnreadMessageCount = getUnreadMessageCount;
        _passwordRecovery = passwordRecovery;
        _login = login;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _permissionCheck = permissionCheck;
        _gridItemLogic = gridItemLogic;
        _knowledgeSummaryLoader = knowledgeSummaryLoader;
        _categoryViewRepo = categoryViewRepo;
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _actionContextAccessor = actionContextAccessor;
        _userReadingRepo = userReadingRepo;
        _questionReadingRepo = questionReadingRepo;
        _jobQueueRepo = jobQueueRepo;
    }

    [HttpPost]
    public JsonResult Login([FromBody] LoginJson loginJson)
    {
        var loginIsSuccessful = _login.UserLogin(loginJson);

        if (loginIsSuccessful)
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
       

        if (!IsEmailAddressAvailable.Yes(json.Email, _userReadingRepo))
            return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.EmailInUse
        });

        if (!IsUserNameAvailable.Yes(json.Email, _userReadingRepo))
            return Json(new RequestResult
        {
            success = false,
            messageKey = FrontendMessageKeys.Error.User.UserNameInUse
        });

        _registerUser.SetUser(json);

        return Json(new RequestResult
        {
            success = true,
            data = new
            {
                IsLoggedIn = _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                Name = _sessionUser.IsLoggedIn? _sessionUser.User.Name : "",
                IsAdmin = _sessionUser.IsInstallationAdmin,
                PersonalWikiId = _sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1,
                Type = UserType.Normal,
                ImgUrl = _sessionUser.IsLoggedIn
                    ? new UserImageSettings(_sessionUser.UserId,
                            _httpContextAccessor)
                        .GetUrl_20px(_sessionUser.User)
                        .Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicControllerLogic(_sessionUser,
                        _permissionCheck, 
                        _gridItemLogic, 
                        _knowledgeSummaryLoader, 
                        _categoryViewRepo, 
                        _imageMetaDataReadingRepo, 
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1)
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

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult RequestVerificationMail()
    {
        SendConfirmationEmail.Run(_sessionUser.User.Id, _jobQueueRepo, _userReadingRepo);
        return Json(new RequestResult
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.User.VerificationMailRequestSent
        });
    }

}