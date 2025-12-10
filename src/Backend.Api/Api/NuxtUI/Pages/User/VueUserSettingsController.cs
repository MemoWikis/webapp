#nullable enable
using Microsoft.AspNetCore.Hosting;

public class VueUserSettingsController(
    SessionUser _sessionUser,
    ReputationUpdate _reputationUpdate,
    CredentialsAreValid _credentialsAreValid,
    UserReadingRepo _userReadingRepo,
    PasswordRecovery _passwordRecovery,
    UserWritingRepo _userWritingRepo,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    QuestionReadingRepo _questionReadingRepo,
    JobQueueRepo _jobQueueRepo,
    DeleteUser _deleteUser,
    PersistentLoginRepo _persistentLoginRepo) : ApiBaseController
{
    public record struct ChangeNotificationIntervalPreferencesResult(
        UIMessage Message,
        bool Success);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeNotificationIntervalPreferencesResult ChangeNotificationIntervalPreferences(
        [FromBody] UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult = UpdateKnowledgeReportInterval.Run(
            _userReadingRepo.GetById(_sessionUser.UserId), 
            notificationInterval, 
            result, 
            _userWritingRepo
        );
        
        var message = updatedResult.ResultMessage;
        if (result.Success && _sessionUser.User.Id == result.AffectedUser.Id)
        {
            _sessionUser.User.KnowledgeReportInterval =
                updatedResult.AffectedUser.KnowledgeReportInterval;
            EntityCache.AddOrUpdate(_sessionUser.User);
            _userWritingRepo.Update(_sessionUser.User);
            return new ChangeNotificationIntervalPreferencesResult
            {
                Success = true,
                Message = message
            };
        }

        return new ChangeNotificationIntervalPreferencesResult
        {
            Success = false
        };
    }

    public class ChangePasswordData
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    };

    public readonly record struct ChangePasswordResult(bool Success, string Message);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangePasswordResult ChangePassword([FromBody] ChangePasswordData data)
    {
        if (!_credentialsAreValid.Yes(_sessionUser.User.EmailAddress, data.CurrentPassword))
            return new ChangePasswordResult
            {
                Success = false,
                Message = FrontendMessageKeys.Error.User.PasswordIsWrong
            };

        if (data.CurrentPassword == data.NewPassword)
            return new ChangePasswordResult
            {
                Success = false,
                Message = FrontendMessageKeys.Error.User.SamePassword
            };

        var user = _userReadingRepo.GetById(_sessionUser.User.Id);
        SetUserPassword.Run(data.NewPassword.Trim(), user);

        return new ChangePasswordResult
        {
            Success = true,
            Message = FrontendMessageKeys.Success.User.PasswordChanged
        };
    }

    public readonly record struct ChangeProfileInformationResult(
        bool Success,
        string MessageKey,
        UserResult Data);

    public readonly record struct UserResult(
        string Name,
        string Email,
        string ImgUrl,
        string TinyImgUrl);

    public class ProfileInformation
    {
        public string? Email { get; set; }
        public IFormFile File { get; set; }
        public int Id { get; set; }
        public string? Username { get; set; }
    };

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeProfileInformationResult ChangeProfileInformation(
        [FromForm] ProfileInformation form)
    {
        if (form.Id != _sessionUser.User.Id)
            return new ChangeProfileInformationResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (form.Email != null)
        {
            var email = form.Email.Trim();
            if (email != _sessionUser.User.EmailAddress &&
                IsEmailAddressAvailable.Yes(form.Email, _userReadingRepo) &&
                IsEmailAdressFormat.Valid(email))
            {
                _sessionUser.User.EmailAddress = email;
                _sessionUser.User.IsEmailConfirmed = false;
            }

            else if (!IsEmailAddressAvailable.Yes(form.Email, _userReadingRepo))
            {
                return new ChangeProfileInformationResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.User.EmailInUse
                };
            }
        }

        if (form.Username != null && form.Username.Trim() != _sessionUser.User.Name &&
            IsUserNameAvailable.Yes(form.Username, _userReadingRepo))
        {
            _sessionUser.User.Name = form.Username.Trim();
        }
        else if (form.Username != null && !IsUserNameAvailable.Yes(form.Username, _userReadingRepo))
        {
            return new ChangeProfileInformationResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.UserNameInUse
            };
        }

        if (form.File != null)
            UpdateUserImage.Run(form.File,
                _sessionUser.UserId,
                _httpContextAccessor,
                _webHostEnvironment);

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        if (form.Email != null && form.Email.Trim() != _sessionUser.User.EmailAddress &&
            !_sessionUser.User.IsEmailConfirmed)
            SendConfirmationEmail.Run(_sessionUser.User.Id, _jobQueueRepo, _userReadingRepo);

        var userImageSettings = new UserImageSettings(_sessionUser.UserId,
            _httpContextAccessor);
        return new ChangeProfileInformationResult
        {
            Success = true,
            MessageKey = FrontendMessageKeys.Success.User.ProfileUpdate,
            Data = new UserResult
            {
                Name = _sessionUser.User.Name,
                Email = _sessionUser.User.EmailAddress,
                ImgUrl = userImageSettings.GetUrl_256px_square(_sessionUser.User).Url,
                TinyImgUrl = userImageSettings.GetUrl_50px_square(_sessionUser.User).Url
            }
        };
    }

    public class ChangeSupportLoginRightsJson
    {
        public bool AllowSupportiveLogin { get; set; }
    };

    public readonly record struct ChangeSupportLoginRightsResult(bool Success, string Message);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeSupportLoginRightsResult ChangeSupportLoginRights(
        [FromBody] ChangeSupportLoginRightsJson json)
    {
        _sessionUser.User.AllowsSupportiveLogin = json.AllowSupportiveLogin;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        return new ChangeSupportLoginRightsResult
        {
            Success = true,
            Message = "supportLoginUpdated"
        };
    }

    public readonly record struct ChangeWishKnowledgeVisibilityJsonResult(bool Success, string Message);

    public class ChangeWishKnowledgeVisibilityJson
    {
        public bool showWishKnowledge { get; set; }
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeWishKnowledgeVisibilityJsonResult ChangeWishKnowledgeVisibility(
        [FromBody] ChangeWishKnowledgeVisibilityJson json)
    {
        _sessionUser.User.ShowWishKnowledge = json.showWishKnowledge;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);
        _reputationUpdate.ForUser(_sessionUser
            .User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        return new ChangeWishKnowledgeVisibilityJsonResult
        {
            Success = true,
            Message = FrontendMessageKeys.Success.User.WishKnowledgeVisibilityUpdated
        };
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public string DeleteUserImage()
    {
        var imageSettings = new UserImageSettings(_httpContextAccessor)
            .InitByType(new ImageMetaData
            {
                Type = ImageType.User,
                TypeId = _sessionUser.User.Id
            }, _questionReadingRepo);
        imageSettings.DeleteFiles();
        return new UserImageSettings(_httpContextAccessor)
            .GetUrl_256px_square(_sessionUser.User).Url;
    }

    [HttpPost]
    public bool ResetPassword()
    {
        var passwordRecoveryResult = _passwordRecovery.Run(_sessionUser.User.EmailAddress);
        return passwordRecoveryResult.Success;
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public bool CanDeleteUser()
    {
        return _deleteUser.CanDelete(_sessionUser.UserId);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public bool DeleteProfile()
    {
        Log.Information("Delete User - Try: {0}", _sessionUser.UserId);

        if (_sessionUser.IsLoggedIn && _deleteUser.CanDelete(_sessionUser.UserId) && _sessionUser.UserId > 0)
        {
            var userId = _sessionUser.UserId;
            RemovePersistentLoginFromCookie.Run(_persistentLoginRepo, _httpContextAccessor.HttpContext);
            RemovePersistentLoginFromCookie.RunForGoogle(_httpContextAccessor.HttpContext);
            _deleteUser.Run(userId);
            _sessionUser.Logout();
            Log.Information("Delete User - Done: {0}", userId);

            return true;
        }

        Log.Warning("Delete User - Fail: {0}", _sessionUser.UserId);
        return false;
    }
}