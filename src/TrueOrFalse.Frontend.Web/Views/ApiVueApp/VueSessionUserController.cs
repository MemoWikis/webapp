﻿using System.IO;
using System.Web.Mvc;

namespace VueApp;

public class VueSessionUserController : BaseController
{
    [HttpPost]
    public JsonResult Login(LoginJson loginJson)
    {
        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(loginJson.EmailAddress, loginJson.Password))
        {

            if (loginJson.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            SessionUser.Login(credentialsAreValid.User);

            TransferActivityPoints.FromSessionToUser();
            Sl.UserRepo.UpdateActivityPointsData();

            return Json(new
            {
                Success = true,
                Message = "",
                Id = SessionUser.UserId,
                WikiId = SessionUser.User.StartTopicId,
                IsAdmin = SessionUser.IsInstallationAdmin,
                Name = SessionUser.User.Name,
                PersonalWikiId = SessionUser.User.StartTopicId
            });
        }

        return Json(new
        {
            Success = false,
            Message = "Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort."
        });
    }

    [HttpPost]
    [AccessOnlyAsLoggedIn]
    public JsonResult LogOut()
    {
        RemovePersistentLoginFromCookie.Run();
        SessionUser.Logout();

        return Json(new
        {
            Success = !SessionUser.IsLoggedIn,
        }, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
    public JsonResult GetCurrentUser()
    {
        var type = UserType.Anonymous;
        if (SessionUser.IsLoggedIn)
        {
            if (SessionUser.User.IsGoogleUser)
                type = UserType.Google;
            else if (SessionUser.User.IsFacebookUser)
                type = UserType.Facebook;
            else type = UserType.Normal;
        }


        var data = new
        {
            IsLoggedIn = SessionUser.IsLoggedIn,
            UserId = SessionUser.UserId,
            Name = SessionUser.IsLoggedIn ? SessionUser.User.Name : "",
            IsAdmin = SessionUser.IsInstallationAdmin,
            WikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : 1,
            Type = type
        };

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    private enum UserType
    {
        Normal,
        Google,
        Facebook,
        Anonymous
    }
}
    
public class StateModel
{
    public bool IsLoggedIn { get; set; }
    public int? UserId { get; set; } = null;

}

public class LoginJson
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool PersistentLogin { get; set; }
}