using System;
using System.Web;
using System.Web.Mvc;

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
    public JsonResult SaveProfileInformations(ProfileInformation form)
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
            var user = Sl.UserRepo.GetById(SessionUser.User.Id);
            SetUserPassword.Run(newPassword.Trim(), user);

            return Json(new
            {
                success = true
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

        return Json(true);
    }

    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(bool allowSupportiveLogin)
    {
        SessionUser.User.AllowsSupportiveLogin = allowSupportiveLogin;

        EntityCache.AddOrUpdate(SessionUser.User);
        Sl.UserRepo.Update(SessionUser.User);

        return Json(true);
    }


    [AccessOnlyAsLoggedIn]
    [HttpPost]
    public JsonResult ChangeSupportLoginRights(UserSettingNotificationInterval notificationInterval)
    {
        DateTime expirationDate;
        var result = UpdateKnowledgeReportInterval.Run(SessionUser.User, notificationInterval, expirationDate, Request["token"]);
        var message = result.ResultMessage;
        if (result.Success && ((UserCacheItem)SessionUser.User).Id == result.AffectedUser.Id)
        {
            SessionUser.User.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
            userSettingsModel.KnowledgeReportInterval = result.AffectedUser.KnowledgeReportInterval;
        }

        EntityCache.AddOrUpdate(SessionUser.User);
        Sl.UserRepo.Update(SessionUser.User);

        return Json(true);
    }

}