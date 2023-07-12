using System.Web;
using System.Web.Mvc;

namespace VueApp;

public class VueUserSettingsController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly UserRepo _userRepo;
    private readonly PasswordRecovery _passwordRecovery;

    public VueUserSettingsController(SessionUser sessionUser,
        ReputationUpdate reputationUpdate,
        CredentialsAreValid credentialsAreValid,
        UserRepo userRepo,
        PasswordRecovery passwordRecovery)
    {
        _sessionUser = sessionUser;
        _reputationUpdate = reputationUpdate;
        _credentialsAreValid = credentialsAreValid;
        _userRepo = userRepo;
        _passwordRecovery = passwordRecovery;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeNotificationIntervalPreferences(UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult =
            UpdateKnowledgeReportInterval.Run(_userRepo.GetById(_sessionUser.UserId), notificationInterval, result, _userRepo);
        var message = updatedResult.ResultMessage;
        if (result.Success && _sessionUser.User.Id == result.AffectedUser.Id)
        {
            _sessionUser.User.KnowledgeReportInterval = updatedResult.AffectedUser.KnowledgeReportInterval;
            EntityCache.AddOrUpdate(_sessionUser.User);
            _userRepo.Update(_sessionUser.User);
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

            var user = _userRepo.GetById(_sessionUser.User.Id);
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

        if (form.email != null && form.email.Trim() != _sessionUser.User.EmailAddress &&
            IsEmailAddressAvailable.Yes(form.email, _userRepo))
        {
            _sessionUser.User.EmailAddress = form.email.Trim();
        }
        else if (form.email != null && !IsEmailAddressAvailable.Yes(form.email, _userRepo))
        {
            return Json(new
                {
                    success = false,
                    message = "emailInUse"
                }
            );
        }

        if (form.username != null && form.username.Trim() != _sessionUser.User.Name &&
            IsUserNameAvailable.Yes(form.username, _userRepo))
        {
            _sessionUser.User.Name = form.username.Trim();
        }
        else if (form.username != null && !IsUserNameAvailable.Yes(form.username, _userRepo))
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
            UserImageStore.Run(form.file, _sessionUser.UserId);
        }

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userRepo.Update(_sessionUser.User);

        return Json(new
        {
            success = true,
            message = "profileUpdate",
            name = _sessionUser.User.Name,
            email = _sessionUser.User.EmailAddress,
            imgUrl = new UserImageSettings(_sessionUser.UserId).GetUrl_250px(_sessionUser.User).Url,
            tinyImgUrl = new UserImageSettings(_sessionUser.UserId).GetUrl_20px(_sessionUser.User).Url
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(bool allowSupportiveLogin)
    {
        _sessionUser.User.AllowsSupportiveLogin = allowSupportiveLogin;

        EntityCache.AddOrUpdate(_sessionUser.User);
        _userRepo.Update(_sessionUser.User);

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
        _userRepo.Update(_sessionUser.User);
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
        var imageSettings = ImageSettings.InitByType(new ImageMetaData
        {
            Type = ImageType.User,
            TypeId = _sessionUser.User.Id
        });
        imageSettings.DeleteFiles();
        return Json(new UserImageSettings().GetUrl_250px(_sessionUser.User).Url, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult ResetPassword()
    {
        var passwordRecoveryResult = _passwordRecovery.Run(_sessionUser.User.EmailAddress);
        return Json(passwordRecoveryResult.Success);
    }

    public class ProfileInformation
    {
        public string email { get; set; } = null;
        public HttpPostedFileBase file { get; set; }
        public int id { get; set; }
        public string username { get; set; } = null;
    }
}