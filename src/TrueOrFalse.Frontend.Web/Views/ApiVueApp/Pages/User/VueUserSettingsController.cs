using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserSettingsController(
    SessionUser _sessionUser,
    ReputationUpdate _reputationUpdate,
    CredentialsAreValid _credentialsAreValid,
    UserReadingRepo _userReadingRepo,
    PasswordRecovery _passwordRecovery,
    UserWritingRepo _userWritingRepo,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    Logg _logg,
    QuestionReadingRepo _questionReadingRepo,
    JobQueueRepo _jobQueueRepo) : Controller
{
    public readonly record struct ChangeNotificationIntervalPreferencesResult(
        UIMessage Message,
        bool Success);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeNotificationIntervalPreferencesResult ChangeNotificationIntervalPreferences(
        [FromBody] UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult =
            UpdateKnowledgeReportInterval.Run(_userReadingRepo.GetById(_sessionUser.UserId),
                notificationInterval, result, _userWritingRepo);
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

    public readonly record struct ChangePasswordData(string currentPassword, string newPassword);

    public readonly record struct PasswordDataResult(bool Success, string Message);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public PasswordDataResult ChangePassword([FromBody] ChangePasswordData data)
    {
        if (!_credentialsAreValid.Yes(_sessionUser.User.EmailAddress, data.currentPassword))
            return new PasswordDataResult
            {
                Success = false,
                Message = "passwordIsWrong"
            };

        if (data.currentPassword == data.newPassword)
            return new PasswordDataResult
            {
                Success = false,
                Message = "samePassword"
            };

        var user = _userReadingRepo.GetById(_sessionUser.User.Id);
        SetUserPassword.Run(data.newPassword.Trim(), user);

        return new PasswordDataResult
        {
            Success = true,
            Message = "passwordChanged"
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

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeProfileInformationResult ChangeProfileInformation(
        [FromForm] ProfileInformation form)
    {
        if (form.id != _sessionUser.User.Id)
            return new ChangeProfileInformationResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.Default
            };

        if (form.email != null)
        {
            var email = form.email.Trim();
            if (email != _sessionUser.User.EmailAddress &&
                IsEmailAddressAvailable.Yes(form.email, _userReadingRepo) &&
                IsEmailAdressFormat.Valid(email))
            {
                _sessionUser.User.EmailAddress = email;
                _sessionUser.User.IsEmailConfirmed = false;
            }

            else if (!IsEmailAddressAvailable.Yes(form.email, _userReadingRepo))
            {
                return new ChangeProfileInformationResult
                {
                    Success = false,
                    MessageKey = FrontendMessageKeys.Error.User.EmailInUse
                };
            }
        }

        if (form.username != null && form.username.Trim() != _sessionUser.User.Name &&
            IsUserNameAvailable.Yes(form.username, _userReadingRepo))
        {
            _sessionUser.User.Name = form.username.Trim();
        }
        else if (form.username != null && !IsUserNameAvailable.Yes(form.username, _userReadingRepo))
        {
            return new ChangeProfileInformationResult
            {
                Success = false,
                MessageKey = FrontendMessageKeys.Error.User.UserNameInUse
            };
        }

        if (form.file != null)
            UpdateUserImage.Run(form.file,
                _sessionUser.UserId,
                _httpContextAccessor,
                _webHostEnvironment,
                _logg);

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        if (form.email != null && form.email.Trim() != _sessionUser.User.EmailAddress &&
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

    public readonly record struct ChangeSupportLoginRightsJson(bool allowSupportiveLogin);

    public readonly record struct ChangeSupportLoginRightsResult(bool Success, string Message);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeSupportLoginRightsResult ChangeSupportLoginRights(
        [FromBody] ChangeSupportLoginRightsJson json)
    {
        _sessionUser.User.AllowsSupportiveLogin = json.allowSupportiveLogin;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        return new ChangeSupportLoginRightsResult
        {
            Success = true,
            Message = "supportLoginUpdated"
        };
    }

    public readonly record struct ChangeWuwiVisibilityJsonResult(bool Success, string Message);

    public readonly record struct ChangeWuwiVisibilityJson(bool showWuwi);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public ChangeWuwiVisibilityJsonResult ChangeWuwiVisibility(
        [FromBody] ChangeWuwiVisibilityJson json)
    {
        _sessionUser.User.ShowWishKnowledge = json.showWuwi;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);
        _reputationUpdate.ForUser(_sessionUser
            .User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        return new ChangeWuwiVisibilityJsonResult
        {
            Success = true,
            Message = "wuwiChanged"
        };
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public string? DeleteUserImage()
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
        var passwordRecoveryResult = _passwordRecovery.RunForNuxt(_sessionUser.User.EmailAddress);
        return passwordRecoveryResult.Success;
    }

    public class ProfileInformation
    {
        public string email { get; set; } = null;
        public IFormFile file { get; set; }
        public int id { get; set; }
        public string username { get; set; } = null;
    }
}