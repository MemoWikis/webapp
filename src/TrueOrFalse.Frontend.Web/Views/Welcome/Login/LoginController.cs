using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class LoginController : BaseController
{
    public ActionResult Login() => View(new LoginModel());

    [HttpPost]
    public JsonResult IsUserNameAvailable(string selectedName) =>
        new JsonResult { Data = new { isAvailable = global::IsUserNameAvailable.Yes(selectedName) } };

    [HttpPost]
    public JsonResult IsEmailAvailable(string selectedEmail) =>
        new JsonResult { Data = new { isAvailable = IsEmailAddressAvailable.Yes(selectedEmail) } };

    [HttpPost]
    public ActionResult Login(LoginModel loginModel)
    {
        loginModel.EmailAddress = loginModel.EmailAddress;
        loginModel.Password = Request["Password"];

        var credentialsAreValid = R<CredentialsAreValid>();

        if (credentialsAreValid.Yes(loginModel.EmailAddress, loginModel.Password))
        {
            if (loginModel.PersistentLogin)
            {
                WritePersistentLoginToCookie.Run(credentialsAreValid.User.Id);
            }

            _sessionUser.Login(credentialsAreValid.User);

            return RedirectToAction(Links.KnowledgeAction, Links.KnowledgeController);
        }

        loginModel.SetInfo_WrongCredentials();

        return View(loginModel);
    }

    public string LoginModal() => 
        ViewRenderer.RenderPartialView("~/Views/Welcome/Login/Login.ascx", new LoginModel(), ControllerContext);
}