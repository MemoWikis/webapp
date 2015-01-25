using System;
using System.Web.Mvc;
using System.Web.UI;
using TrueOrFalse;
using TrueOrFalse.Registration;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeController : BaseController
{
    private readonly RegisterUser _registerUser;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly WritePersistentLoginToCookie _writePersistentLoginToCookie;
        
    public WelcomeController(RegisterUser registerUser, 
                             CredentialsAreValid credentialsAreValid, 
                             WritePersistentLoginToCookie writePersistentLoginToCookie)
    {
        _registerUser = registerUser;
        _credentialsAreValid = credentialsAreValid;
        _writePersistentLoginToCookie = writePersistentLoginToCookie;
    }

    public ActionResult Welcome(){
        return View(new WelcomeModel());
    }

    public ActionResult Welcome2(){
        return View(new WelcomeModel());
    }

    public ActionResult Register(){ return View(new RegisterModel()); }
        
    [HttpPost]
    public ActionResult Register(RegisterModel model){

        if (!R<IsEmailAddressAvailable>().Yes(model.Email))
            ModelState.AddModelError("Email", "Die Emailadresse ist bereits vergeben");

        if (!ModelState.IsValid) 
            return View(model);
        
        _registerUser.Run(RegisterModelToUser.Run(model));
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

            if(loginModel.PersistentLogin){
                _writePersistentLoginToCookie.Run(_credentialsAreValid.User.Id);
            }
            
            _sessionUser.Login(_credentialsAreValid.User);

            return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
        }

        loginModel.SetToWrongCredentials();

        return View(loginModel);
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
            model.Message = new SuccessMessage("Der Link wurde an " + model.Email + " verschickt.");

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
            model.Message = new ErrorMessage("Der Link ist leider ungültig.");

        return View(model);
    }

    [HttpPost]
    public ActionResult PasswordReset(PasswordResetModel model)
    {
        if(!ModelState.IsValid)
        {
            model.TokenFound = true;
            model.Message = new ErrorMessage("Bitte prüfe deine Eingabe.");
            return View(model);
        }

        var result = Sl.Resolve<PasswordResetPrepare>().Run(model.Token);

        var userRepo = Sl.Resolve<UserRepository>();
        var user = userRepo.GetByEmail(result.Email);

        if(user == null)
            throw new Exception();

        SetUserPassword.Run(model.NewPassword1, user);
        userRepo.Update(user);

        Sl.Resolve<SessionUser>().Login(user);

        return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
    }
}