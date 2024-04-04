using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.User;
namespace VueApp;

public class UserStoreControllerVueSessionUser( VueSessionUser _vueSessionUser,
SessionUser _sessionUser,
RegisterUser _registerUser,
PersistentLoginRepo _persistentLoginRepo,
GetUnreadMessageCount _getUnreadMessageCount,
PasswordRecovery _passwordRecovery,
Login _login,
IHttpContextAccessor _httpContextAccessor,
PermissionCheck _permissionCheck,
TopicGridManager _gridItemLogic,
KnowledgeSummaryLoader _knowledgeSummaryLoader,
CategoryViewRepo _categoryViewRepo,
ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
UserReadingRepo _userReadingRepo,
QuestionReadingRepo _questionReadingRepo,
JobQueueRepo _jobQueueRepo) : BaseController(_sessionUser)
{
    [HttpPost]
    public JsonResult Login([FromBody] LoginParam param)
    {
        var loginIsSuccessful = _login.UserLogin(param);

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
                        .GetUrl_20px_square(_sessionUser.User)
                        .Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = new TopicDataManager(_sessionUser,
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