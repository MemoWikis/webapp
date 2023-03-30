using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class UserStoreController : BaseController
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
                CurrentUser = VueSessionUser.GetCurrentUserData()
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
    [AccessOnlyAsLoggedIn]
    public int GetUnreadMessagesCount()
    {
        return Sl.Resolve<GetUnreadMessageCount>().Run(SessionUser.UserId);
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