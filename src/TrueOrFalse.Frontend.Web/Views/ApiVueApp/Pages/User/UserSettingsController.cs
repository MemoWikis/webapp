using System.Web;
using System.Web.Mvc;

namespace VueApp;

public class VueUserSettingsController : BaseController
{
    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeNotificationIntervalPreferences(UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult =
            UpdateKnowledgeReportInterval.Run(Sl.UserRepo.GetById(SessionUserLegacy.UserId), notificationInterval, result);
        var message = updatedResult.ResultMessage;
        if (result.Success && SessionUserLegacy.User.Id == result.AffectedUser.Id)
        {
            SessionUserLegacy.User.KnowledgeReportInterval = updatedResult.AffectedUser.KnowledgeReportInterval;
            EntityCache.AddOrUpdate(SessionUserLegacy.User);
            Sl.UserRepo.Update(SessionUserLegacy.User);
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
        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(SessionUserLegacy.User.EmailAddress, currentPassword))
        {
            if (currentPassword == newPassword)

            {
                return Json(new
                {
                    success = false,
                    message = "samePassword"
                });
            }

            var user = Sl.UserRepo.GetById(SessionUserLegacy.User.Id);
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
        if (form.id != SessionUserLegacy.User.Id)
        {
            return Json(null);
        }

        if (form.email != null && form.email.Trim() != SessionUserLegacy.User.EmailAddress &&
            IsEmailAddressAvailable.Yes(form.email))
        {
            SessionUserLegacy.User.EmailAddress = form.email.Trim();
        }
        else if (form.email != null && !IsEmailAddressAvailable.Yes(form.email))
        {
            return Json(new
                {
                    success = false,
                    message = "emailInUse"
                }
            );
        }

        if (form.username != null && form.username.Trim() != SessionUserLegacy.User.Name &&
            IsUserNameAvailable.Yes(form.username))
        {
            SessionUserLegacy.User.Name = form.username.Trim();
        }
        else if (form.username != null && !IsUserNameAvailable.Yes(form.username))
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
            UserImageStore.Run(form.file, SessionUserLegacy.UserId);
        }

        EntityCache.AddOrUpdate(SessionUserLegacy.User);
        Sl.UserRepo.Update(SessionUserLegacy.User);

        return Json(new
        {
            success = true,
            message = "profileUpdate",
            name = SessionUserLegacy.User.Name,
            email = SessionUserLegacy.User.EmailAddress,
            imgUrl = new UserImageSettings(SessionUserLegacy.UserId).GetUrl_250px(SessionUserLegacy.User).Url,
            tinyImgUrl = new UserImageSettings(SessionUserLegacy.UserId).GetUrl_20px(SessionUserLegacy.User).Url
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(bool allowSupportiveLogin)
    {
        SessionUserLegacy.User.AllowsSupportiveLogin = allowSupportiveLogin;

        EntityCache.AddOrUpdate(SessionUserLegacy.User);
        Sl.UserRepo.Update(SessionUserLegacy.User);

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
        SessionUserLegacy.User.ShowWishKnowledge = showWuwi;

        EntityCache.AddOrUpdate(SessionUserLegacy.User);
        Sl.UserRepo.Update(SessionUserLegacy.User);
        ReputationUpdate.ForUser(SessionUserLegacy
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
            TypeId = SessionUserLegacy.User.Id
        });
        imageSettings.DeleteFiles();
        return Json(new UserImageSettings().GetUrl_250px(SessionUserLegacy.User).Url, JsonRequestBehavior.AllowGet);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ResetPassword()
    {
        var passwordRecoveryResult = Sl.Resolve<PasswordRecovery>().Run(SessionUserLegacy.User.EmailAddress);
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