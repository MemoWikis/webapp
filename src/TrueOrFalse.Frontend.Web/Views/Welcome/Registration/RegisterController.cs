using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class RegisterController : BaseController
{
    private string _viewRegisterPath = "~/Views/Welcome/Registration/Register.aspx";
    private string _viewRegisterSuccessPath = "~/Views/Welcome/Registration/RegisterSuccess.aspx";

    public ActionResult Register() { return View(_viewRegisterPath, new RegisterModel()); }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
        if (!IsEmailAddressAvailable.Yes(model.Email))
            ModelState.AddModelError("E-Mail", "Diese E-Mail-Adresse ist bereits registriert.");

        if (!global::IsUserNameAvailable.Yes(model.Name))
            ModelState.AddModelError("Name", "Dieser Benutzername ist bereits vergeben.");

        if (!ModelState.IsValid)
            return View(model);

        var user = RegisterModelToUser.Run(model);

        RegisterUser.Run(user);

        _sessionUser.Login(user);

        return RedirectToAction(Links.RegisterSuccess, Links.RegisterController);
    }

    public ActionResult RegisterSuccess() => View(_viewRegisterSuccessPath, new RegisterSuccessModel());
}