using System;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class VueUserSettingsController : BaseController
{
    public class ProfileInformation
    {
        public int id { get; set; }
        public HttpPostedFileBase file { get; set; }
        public string username { get; set; } = null;
        public string email { get; set; } = null;
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeProfileInformation(ProfileInformation form)
    {
        if (form.id != SessionUser.User.Id)
            return Json(null);

        if (form.email != null && form.email.Trim() != SessionUser.User.EmailAddress && IsEmailAddressAvailable.Yes(form.email))
            SessionUser.User.EmailAddress = form.email.Trim();
        else if(form.email != null && !IsEmailAddressAvailable.Yes(form.email))
            return Json(new
                {
                    success = false,
                    message = "emailInUse",
                }
            );

        if (form.username != null && form.username.Trim() != SessionUser.User.Name && IsUserNameAvailable.Yes(form.username))
            SessionUser.User.Name = form.username.Trim();
        else if (form.username != null && !IsUserNameAvailable.Yes(form.username))
            return Json(new
                {
                    success = false,
                    message = "userNameInUse",
                }
            );

        if (form.file != null)
            UserImageStore.Run(form.file, SessionUser.UserId);

        EntityCache.AddOrUpdate(SessionUser.User);
        Sl.UserRepo.Update(SessionUser.User);

        return Json(new
        {
            success = true,
            message = "profileUpdate",
            name = SessionUser.User.Name,
            email = SessionUser.User.EmailAddress,
            imgUrl = new UserImageSettings(SessionUser.UserId).GetUrl_250px(SessionUser.User).Url,
            tinyImgUrl = new UserImageSettings(SessionUser.UserId).GetUrl_20px(SessionUser.User).Url
        });
    }


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangePassword(string currentPassword, string newPassword)
    {
        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(SessionUser.User.EmailAddress, currentPassword))
        {
            if (currentPassword == newPassword)

                return Json(new
                {
                    success = false,
                    message = "samePassword"
                });

            var user = Sl.UserRepo.GetById(SessionUser.User.Id);
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
    public JsonResult ChangeWuwiVisibility(bool showWuwi)
    {
        SessionUser.User.ShowWishKnowledge = showWuwi;

        EntityCache.AddOrUpdate(SessionUser.User);
        Sl.UserRepo.Update(SessionUser.User);
        ReputationUpdate.ForUser(SessionUser.User); //setting of ShowWishKnowledge affects reputation of user -> needs recalculation

        return Json(new
        {
            success = true,
            message = "wuwiChanged"
        });
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(bool allowSupportiveLogin)
    {
        SessionUser.User.AllowsSupportiveLogin = allowSupportiveLogin;

        EntityCache.AddOrUpdate(SessionUser.User);
        Sl.UserRepo.Update(SessionUser.User);

        return Json(new
        {
            success = true,
            message = "supportLoginUpdated"
        });
    }


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeNotificationIntervalPreferences(UserSettingNotificationInterval notificationInterval)
    {
        var result = new UpdateKnowledgeReportIntervalResult();
        var updatedResult = UpdateKnowledgeReportInterval.Run(Sl.UserRepo.GetById(SessionUser.UserId), notificationInterval, result);
        var message = updatedResult.ResultMessage;
        if (result.Success && SessionUser.User.Id == result.AffectedUser.Id)
        {
            SessionUser.User.KnowledgeReportInterval = updatedResult.AffectedUser.KnowledgeReportInterval;
            EntityCache.AddOrUpdate(SessionUser.User);
            Sl.UserRepo.Update(SessionUser.User);
            return Json(new
            {
                success = true,
                message = message,
            });
        }

        return Json(new
        {
            success = false
        });

    }

}