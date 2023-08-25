using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class VueUserSettingsController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly PasswordRecovery _passwordRecovery;
    private readonly UserWritingRepo _userWritingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public VueUserSettingsController(SessionUser sessionUser,
        ReputationUpdate reputationUpdate,
        CredentialsAreValid credentialsAreValid,
        UserReadingRepo userReadingRepo,
        PasswordRecovery passwordRecovery,
        UserWritingRepo userWritingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _sessionUser = sessionUser;
        _reputationUpdate = reputationUpdate;
        _credentialsAreValid = credentialsAreValid;
        _userReadingRepo = userReadingRepo;
        _passwordRecovery = passwordRecovery;
        _userWritingRepo = userWritingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeNotificationIntervalPreferences(UserSettingNotificationInterval notificationInterval)
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
                success = true, message
            });
        }

        return Json(new
        {
            success = false
        });
    }


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangePassword(string currentPassword, string newPassword)
    {
        if (_credentialsAreValid.Yes(_sessionUser.User.EmailAddress, currentPassword))
        {
            if (currentPassword == newPassword)

            {
                return Json(new
                {
                    success = false,
                    message = "samePassword"
                });
            }

            var user = _userReadingRepo.GetById(_sessionUser.User.Id);
            SetUserPassword.Run(newPassword.Trim(), user);

            return Json(new
            {
                success = true,
                message = "passwordChanged"
            });
        }


        return Json(new
        {
            success = false,
            message = "passwordIsWrong"
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeProfileInformation(ProfileInformation form)
    {
        if (form.id != _sessionUser.User.Id)
        {
            return Json(null);
        }

        if(string.IsNullOrEmpty(form.email))
            return Json(new
                {
                    success = false,
                    message = "emailEmpty"
                }
            );
        
        var email = form.email.Trim();
        if (email != _sessionUser.User.EmailAddress &&
            IsEmailAddressAvailable.Yes(form.email, _userReadingRepo) &&
            IsEmailAdressFormat.Valid(email))
        {
            _sessionUser.User.EmailAddress = email;
        }

        else if (form.email != null && !IsEmailAddressAvailable.Yes(form.email, _userReadingRepo))
        {
            return Json(new
                {
                    success = false,
                    message = "emailInUse"
                }
            );
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
                    message = "userNameInUse"
                }
            );
        }

        if (form.file != null)
        {
            UserImageStore.Run(form.file, 
                _sessionUser.UserId,
                _httpContextAccessor,
                _webHostEnvironment);
        }

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        var userImageSettings = new UserImageSettings(_sessionUser.UserId, 
            _httpContextAccessor, 
            _webHostEnvironment);
        return Json(new
        {
            success = true,
            message = "profileUpdate",
            name = _sessionUser.User.Name,
            email = _sessionUser.User.EmailAddress,
            imgUrl = userImageSettings.GetUrl_250px(_sessionUser.User).Url,
            tinyImgUrl = userImageSettings.GetUrl_20px(_sessionUser.User).Url
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(bool allowSupportiveLogin)
    {
        _sessionUser.User.AllowsSupportiveLogin = allowSupportiveLogin;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userWritingRepo.Update(_sessionUser.User);

        return Json(new
        {
            success = true,
            message = "supportLoginUpdated"
        });
    }


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeWuwiVisibility(bool showWuwi)
    {
        _sessionUser.User.ShowWishKnowledge = showWuwi;

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
        var imageSettings = new ImageSettings(_httpContextAccessor, _webHostEnvironment)
            .InitByType(new ImageMetaData
        {
            Type = ImageType.User,
            TypeId = _sessionUser.User.Id
        });
        imageSettings.DeleteFiles();
        return Json(new UserImageSettings(_httpContextAccessor, _webHostEnvironment).GetUrl_250px(_sessionUser.User).Url);
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