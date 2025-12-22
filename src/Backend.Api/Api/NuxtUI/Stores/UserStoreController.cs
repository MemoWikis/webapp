
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
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    JobQueueRepo _jobQueueRepo,
    UserUiLanguage _userUiLanguage,
    TokenDeductionService _tokenDeductionService) : ApiBaseController
{
    public readonly record struct LoginResponse(
        FrontEndUserData.CurrentUserData Data,
        string MessageKey,
        bool Success);

    [HttpPost]
    public LoginResponse Login([FromBody] LoginRequest request)
    {
        var loginIsSuccessful = _login.UserLogin(request);

        if (loginIsSuccessful)
        {
            RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);

            return new LoginResponse
            {
                Success = true,
                Data = _frontEndUserData.Get()
            };
        }

        return new LoginResponse
        {
            Success = false,
            MessageKey = FrontendMessageKeys.Error.User.LoginFailed
        };
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public LoginResponse LogOut()
    {
        RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor.HttpContext);
        RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);
        _sessionUser.Logout();

        if (!_sessionUser.IsLoggedIn)
            return new LoginResponse
            {
                Success = true,
            };

        return new LoginResponse
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

    public readonly record struct ResetPasswordRequest(string Email);

    [HttpPost]
    public LoginResponse ResetPassword([FromBody] ResetPasswordRequest req)
    {
        var result = _passwordRecovery.Run(req.Email);
        //Don't reveal if email exists 
        return new LoginResponse { Success = result.Success || result.EmailDoesNotExist };
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
        PageDataManager.PageDataResult PersonalWiki,
        FrontEndUserData.ActivityPoints ActivityPoints,
        string CollaborationToken);


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
                PersonalWikiId = _sessionUser.FirstWikiId(),
                Type = UserType.Normal,
                ImgUrl = _sessionUser.IsLoggedIn
                    ? new UserImageSettings(_sessionUser.UserId,
                            _httpContextAccessor)
                        .GetUrl_20px_square(_sessionUser.User)
                        .Url
                    : "",
                Reputation = _sessionUser.IsLoggedIn ? _sessionUser.User.Reputation : 0,
                ReputationPos = _sessionUser.IsLoggedIn ? _sessionUser.User.ReputationPos : 0,
                PersonalWiki = new PageDataManager(_sessionUser,
                        _permissionCheck,
                        _knowledgeSummaryLoader,
                        _imageMetaDataReadingRepo,
                        _httpContextAccessor,
                        _questionReadingRepo)
                    .GetPageData(_sessionUser.FirstWikiId()),
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
                },
                CollaborationToken = new CollaborationToken().Get(_sessionUser.UserId)
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

    public readonly record struct UpdateLanguageSettingRequest(string Language);

    public readonly record struct UpdateLanguageSettingResponse(string Language);


    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public UpdateLanguageSettingResponse UpdateLanguageSetting([FromBody] UpdateLanguageSettingRequest req)
    {
        _userUiLanguage.SetUiLanguage(_sessionUser.UserId, req.Language);

        return new UpdateLanguageSettingResponse();
    }

    public readonly record struct AddShareTokenRequest(int PageId, string ShareToken);

    [HttpPost]
    public void AddShareToken([FromBody] AddShareTokenRequest request)
    {
        _sessionUser.AddShareToken(request.PageId, request.ShareToken);
    }

    public readonly record struct GetTokenBalanceResponse(
        bool Success,
        int TotalBalance);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public GetTokenBalanceResponse GetTokenBalance()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GetTokenBalanceResponse(false, 0);
        }

        var balance = _tokenDeductionService.GetTotalTokenBalance(_sessionUser.UserId);
        return new GetTokenBalanceResponse(true, balance);
    }
}