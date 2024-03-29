﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class VueUserSettingsController : BaseController
{
    private readonly ReputationUpdate _reputationUpdate;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly PasswordRecovery _passwordRecovery;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly Logg _logg;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly JobQueueRepo _jobQueueRepo;

    public VueUserSettingsController(SessionUser sessionUser,
        ReputationUpdate reputationUpdate,
        CredentialsAreValid credentialsAreValid,
        UserReadingRepo userReadingRepo,
        PasswordRecovery passwordRecovery,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        Logg logg,
        QuestionReadingRepo questionReadingRepo,
        JobQueueRepo jobQueueRepo) : base(sessionUser)
    {
        _reputationUpdate = reputationUpdate;
        _credentialsAreValid = credentialsAreValid;
        _userReadingRepo = userReadingRepo;
        _passwordRecovery = passwordRecovery;
        _userWritingRepo = userWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _logg = logg;
        _questionReadingRepo = questionReadingRepo;
        _jobQueueRepo = jobQueueRepo;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeNotificationIntervalPreferences([FromBody] UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult =
            UpdateKnowledgeReportInterval.Run(_userReadingRepo.GetById(_sessionUser.UserId), notificationInterval, result, _userWritingRepo);
        var message = updatedResult.ResultMessage;
        if (result.Success && _sessionUser.User.Id == result.AffectedUser.Id)
        {
            _sessionUser.User.KnowledgeReportInterval = updatedResult.AffectedUser.KnowledgeReportInterval;
            EntityCache.AddOrUpdate(_sessionUser.User);
            _userWritingRepo.Update(_sessionUser.User);
            return Json(new
            {
                success = true,
                message
            });
        }

        return Json(new
        {
            success = false
        });
    }

    public readonly record struct ChangePasswordData(string currentPassword, string newPassword);

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangePassword([FromBody] ChangePasswordData data)
    {
        if (!_credentialsAreValid.Yes(_sessionUser.User.EmailAddress, data.currentPassword))
            return Json(new
            {
                success = false,
                message = "passwordIsWrong"
            });

        if (data.currentPassword == data.newPassword)
            return Json(new
            {
                success = false,
                message = "samePassword"
            });

        var user = _userReadingRepo.GetById(_sessionUser.User.Id);
        SetUserPassword.Run(data.newPassword.Trim(), user);

        return Json(new
        {
            success = true,
            message = "passwordChanged"
        });


    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeProfileInformation([FromForm] ProfileInformation form)
    {
        if (form.id != _sessionUser.User.Id)
            return Json(new RequestResult
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.Default
            });

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
                return Json(new
                {
                    success = false,
                    messageKey = FrontendMessageKeys.Error.User.EmailInUse
                });
            }
        }


        if (form.username != null && form.username.Trim() != _sessionUser.User.Name &&
            IsUserNameAvailable.Yes(form.username, _userReadingRepo))
        {
            _sessionUser.User.Name = form.username.Trim();
        }
        else if (form.username != null && !IsUserNameAvailable.Yes(form.username, _userReadingRepo))
        {
            return Json(new
            {
                success = false,
                messageKey = FrontendMessageKeys.Error.User.UserNameInUse
            }
            );
        }

        if (form.file != null)
            UpdateUserImage.Run(form.file,
                _sessionUser.UserId,
                _httpContextAccessor,
                _webHostEnvironment,
                _logg);

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        if (form.email != null && form.email.Trim() != _sessionUser.User.EmailAddress && !_sessionUser.User.IsEmailConfirmed)
            SendConfirmationEmail.Run(_sessionUser.User.Id, _jobQueueRepo, _userReadingRepo);

        var userImageSettings = new UserImageSettings(_sessionUser.UserId,
            _httpContextAccessor);
        return Json(new
        {
            success = true,
            messageKey = FrontendMessageKeys.Success.User.ProfileUpdate,
            data = new
            {
                name = _sessionUser.User.Name,
                email = _sessionUser.User.EmailAddress,
                imgUrl = userImageSettings.GetUrl_256px_square(_sessionUser.User).Url,
                tinyImgUrl = userImageSettings.GetUrl_50px_square(_sessionUser.User).Url
            }
        });
    }

    public readonly record struct ChangeSupportLoginRightsJson(bool allowSupportiveLogin);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights([FromBody] ChangeSupportLoginRightsJson json)
    {
        _sessionUser.User.AllowsSupportiveLogin = json.allowSupportiveLogin;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        return Json(new
        {
            success = true,
            message = "supportLoginUpdated"
        });
    }

    public readonly record struct ChangeWuwiVisibilityJson(bool showWuwi);
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeWuwiVisibility([FromBody] ChangeWuwiVisibilityJson json)
    {
        _sessionUser.User.ShowWishKnowledge = json.showWuwi;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);
        _reputationUpdate.ForUser(_sessionUser
            .User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        return Json(new
        {
            success = true,
            message = "wuwiChanged"
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public JsonResult DeleteUserImage()
    {
        var imageSettings = new UserImageSettings(_httpContextAccessor)
            .InitByType(new ImageMetaData
            {
                Type = ImageType.User,
                TypeId = _sessionUser.User.Id
            }, _questionReadingRepo);
        imageSettings.DeleteFiles();
        return Json(new UserImageSettings(_httpContextAccessor).GetUrl_256px_square(_sessionUser.User).Url);
    }

    [HttpPost]
    public JsonResult ResetPassword()
    {
        var passwordRecoveryResult = _passwordRecovery.RunForNuxt(_sessionUser.User.EmailAddress);
        return Json(passwordRecoveryResult.Success);
    }

    public class ProfileInformation
    {
        public string email { get; set; } = null;
        public IFormFile file { get; set; }
        public int id { get; set; }
        public string username { get; set; } = null;
    }
}