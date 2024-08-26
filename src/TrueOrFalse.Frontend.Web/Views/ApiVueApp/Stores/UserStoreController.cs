using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.User;

namespace VueApp;

public class UserStoreController(
    FrontEndUserData _frontEndUserData,
    SessionUser _sessionUser,
    RegisterUser _registerUser,
    PersistentLoginRepo _persistentLoginRepo,
    GetUnreadMessageCount _getUnreadMessageCount,
    PasswordRecovery _passwordRecovery,
    Login _login,
    IHttpContextAccessor _httpContextAccessor,
    PermissionCheck _permissionCheck,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    CategoryViewRepo _categoryViewRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    JobQueueRepo _jobQueueRepo) : BaseController(_sessionUser)
{
    public readonly record struct LoginResult(
        FrontEndUserData.CurrentUserData Data,
        string MessageKey,
        bool Success);

    [HttpPost]
    public LoginResult Login([FromBody] LoginParam param)
    {
        var loginIsSuccessful = _login.UserLogin(param);

        if (loginIsSuccessful)
        {
            RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);

            return new LoginResult
            {
                Success = true,
                Data = _frontEndUserData.Get()
            };
        }

        return new LoginResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.LoginFailed
        };
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public LoginResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor.HttpContext);
        RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);
        _sessionUser.Logout();
        //delete aspnetsession cookie
        //_httpContextAccessor.HttpContext.Response.Cookies.Delete("key");

        if (!_sessionUser.IsLoggedIn)
            return new LoginResult
            {
                Success = true,
            };

        return new LoginResult
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.Default
        };
    }

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return _getUnreadMessageCount.Run(_sessionUser.UserId);
    }

    [HttpPost]
    public LoginResult ResetPassword(string email)
    {
        var result = _passwordRecovery.RunForNuxt(email);
        //Don't reveal if email exists 
        return new LoginResult { Success = result.Success || result.EmailDoesNotExist };
    }

    public readonly record struct RegisterResult(
        bool Success,
        RegisterDetails Data,
        string MessageKey);

    public readonly record struct RegisterDetails(
        bool IsLoggedIn,
        int Id,
        string Name,
        bool IsAdmin,
        int PersonalWikiId,
        UserType Type,
        string ImgUrl,
        int Reputation,
        int ReputationPos,
        TopicDataManager.TopicDataResult PersonalWiki,
        FrontEndUserData.ActivityPoints ActivityPoints);


    [HttpPost]
    public RegisterResult Register([FromBody] RegisterJson json)
    {
        if (!IsEmailAddressAvailable.Yes(json.Email, _userReadingRepo))
            return new RegisterResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.EmailInUse
            };

        if (!IsUserNameAvailable.Yes(json.Email, _userReadingRepo))
            return new RegisterResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.UserNameInUse
            };

        _registerUser.SetUser(json);
        var activityPoints = _sessionUser.User.ActivityPoints; 
        var activityLevel = _sessionUser.User.ActivityLevel;
        RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);

        return new RegisterResult
        {
            Success = true,
            Data = new RegisterDetails
            {
                IsLoggedIn = _sessionUser.IsLoggedIn,
                Id = _sessionUser.UserId,
                Name = _sessionUser.IsLoggedIn ? _sessionUser.User.Name : "",
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
                        _knowledgeSummaryLoader,
                        _categoryViewRepo,
                        _imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetTopicData(_sessionUser.IsLoggedIn ? _sessionUser.User.StartTopicId : 1),
                ActivityPoints = new FrontEndUserData.ActivityPoints
                {
                    Points = activityPoints,
                    Level = activityLevel,
                    LevelUp = false,
                    ActivityPointsTillNextLevel =
                        UserLevelCalculator.GetUpperLevelBound(activityLevel) - activityPoints,
                    ActivityPointsPercentageOfNextLevel = activityPoints == 0
                        ? 0
                        : 100 * activityPoints /
                          UserLevelCalculator.GetUpperLevelBound(activityLevel)
                }
            }
        };
    }

    public readonly record struct RequestVerificationMailResult(
        string MessageKey,
        bool Success);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public RequestVerificationMailResult RequestVerificationMail()
    {
        SendConfirmationEmail.Run(_sessionUser.User.Id, _jobQueueRepo, _userReadingRepo);
        return new RequestVerificationMailResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.User.VerificationMailRequestSent
        };
    }
}