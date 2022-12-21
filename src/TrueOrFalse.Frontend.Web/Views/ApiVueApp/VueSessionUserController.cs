using System.IO;
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
                CurrentUser = GetCurrentUser()
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

        return Json(new
        {
            IsLoggedIn = SessionUser.IsLoggedIn,
            Id = IsLoggedIn ? SessionUser.UserId : -1,
            Name = SessionUser.IsLoggedIn ? SessionUser.User.Name : "",
            IsAdmin = SessionUser.IsInstallationAdmin,
            PersonalWikiId = SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : RootCategory.RootCategoryId,
            Type = type,
            ImgUrl = SessionUser.IsLoggedIn
                ? new UserImageSettings(SessionUser.UserId).GetUrl_20px(SessionUser.User).Url
                : "",
            Reputation = SessionUser.IsLoggedIn ? SessionUser.User.Reputation : 0,
            ReputationPos = SessionUser.IsLoggedIn ? SessionUser.User.ReputationPos : 0,
            PersonalWiki = new TopicController().GetTopic(SessionUser.IsLoggedIn ? SessionUser.User.StartTopicId : RootCategory.RootCategoryId)
        }, JsonRequestBehavior.AllowGet);
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