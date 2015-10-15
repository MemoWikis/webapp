using System;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class WelcomeController : BaseController
{
    private readonly RegisterUser _registerUser;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly WritePersistentLoginToCookie _writePersistentLoginToCookie;
    private readonly RemovePersistentLoginFromCookie _removePersistentLoginFromCookie;

    public WelcomeController(RegisterUser registerUser,
                             CredentialsAreValid credentialsAreValid,
                             WritePersistentLoginToCookie writePersistentLoginToCookie,
                             RemovePersistentLoginFromCookie removePersistentLoginFromCookie)
    {
        _registerUser = registerUser;
        _credentialsAreValid = credentialsAreValid;
        _writePersistentLoginToCookie = writePersistentLoginToCookie;
        _removePersistentLoginFromCookie = removePersistentLoginFromCookie;
    }
    
    public ActionResult Welcome(){
        return View(new WelcomeModel());
    }

    public ActionResult LogOn()
    {
        return View();
    }

    public ActionResult Logout()
    {
        _removePersistentLoginFromCookie.Run();
        _sessionUser.Logout();
        return View(new BaseModel());
    }

    public ActionResult Register() { return View(new RegisterModel()); }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {

        if (!ServiceLocator.R<IsEmailAddressAvailable>().Yes(model.Email))
            ModelState.AddModelError("Email", "Die Emailadresse ist bereits vergeben.");

        if (!ModelState.IsValid)
            return View(model);

        var user = RegisterModelToUser.Run(model);
        _registerUser.Run(user);

        _sessionUser.Login(user);

        return RedirectToAction(Links.RegisterSuccess, Links.WelcomeController);
    }

    public ActionResult RegisterSuccess() { return View(new RegisterSuccessModel()); }

    public ActionResult Login()
    {
        return View(new LoginModel());
    }

    [HttpPost]
    public ActionResult Login(LoginModel loginModel)
    {
        loginModel.EmailAddress = loginModel.EmailAddress;
        loginModel.Password = Request["Password"];

        if (_credentialsAreValid.Yes(loginModel.EmailAddress, loginModel.Password))
        {
            if (loginModel.PersistentLogin)
            {
                _writePersistentLoginToCookie.Run(_credentialsAreValid.User.Id);
            }

            _sessionUser.Login(_credentialsAreValid.User);

            return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
        }

        loginModel.SetToWrongCredentials();

        return View(loginModel);
    }

    //For Tool.Muse and SignalRClients
    public JsonResult RemoteLogin(string userName, string password)
    {
        if (userName == Settings.SignalrUser() && password == Settings.SignalrPassword())
        {
            _sessionUser.Login(new User{
                Id = -1,
                Name = "SIGNALR-LOGIN",
                IsInstallationAdmin = false
            });
            return new JsonResult { Data = new { UserId = -1 } };    
        }

        var userId = -1;
        if (_credentialsAreValid.Yes(userName, password))
        {
            _sessionUser.Login(_credentialsAreValid.User);
            userId = _credentialsAreValid.User.Id;
        }
        else
            Response.StatusCode = 401; 

        return new JsonResult{Data = new {UserId = userId}};
    }

    public ActionResult PasswordRecovery()
    {
        return View(new PasswordRecoveryModel());
    }

    [HttpPost]
    public ActionResult PasswordRecovery(PasswordRecoveryModel model)
    {
        var result = Sl.Resolve<PasswordRecovery>().Run(model.Email);
        if (result.TheEmailDoesNotExist)
            model.Message = new ErrorMessage("Diese Email-Adresse ist uns unbekannt.");
        else if (result.Success)
            model.Message = new SuccessMessage("Wir haben an " + model.Email + " eine Email verschickt (überprüfe gegebenenfalls auch deinen Spam-Ordner). Klicke dort auf den Link, um ein neues Passwort zu setzen.");

        return View(model);
    }

    [HttpGet]
    public ActionResult PasswordReset(string id)
    {
        var result = Sl.Resolve<PasswordResetPrepare>().Run(id);
        var model = new PasswordResetModel { TokenFound = result.Success, Token = id };

        if (result.TokenOlderThan72h)
        {
            model.Message = new ErrorMessage("Der Link ist abgelaufen.");
            return View(model);
        }

        if (!result.Success)
            model.Message = new ErrorMessage("Der Link ist leider ungültig. Wenn du Probleme hast, schreibe uns einfach eine Email an team@memucho.de.");

        return View(model);
    }

    [HttpPost]
    public ActionResult PasswordReset(PasswordResetModel model)
    {
        if (!ModelState.IsValid)
        {
            model.TokenFound = true;
            model.Message = new ErrorMessage("Bitte prüfe deine Eingabe.");
            return View(model);
        }

        var result = Sl.Resolve<PasswordResetPrepare>().Run(model.Token);

        var userRepo = Sl.Resolve<UserRepo>();
        var user = userRepo.GetByEmail(result.Email);

        if (user == null)
            throw new Exception();

        SetUserPassword.Run(model.NewPassword1, user);
        userRepo.Update(user);

        _sessionUser.Login(user);

        return RedirectToAction(Links.Knowledge, Links.KnowledgeController, new {passwordSet = "true"});
    }
}