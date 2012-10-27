using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;

[HandleError]
public class WelcomeController : Controller
{
    private readonly RegisterUser _registerUser;
    private readonly CredentialsAreValid _credentialsAreValid;
    private readonly WritePersistentLoginToCookie _writePersistentLoginToCookie;
    private readonly SessionUser _sessionUser;
        
    public WelcomeController(RegisterUser registerUser, 
                             CredentialsAreValid credentialsAreValid, 
                             WritePersistentLoginToCookie writePersistentLoginToCookie, 
                             SessionUser sessionUser)
    {
        _registerUser = registerUser;
        _credentialsAreValid = credentialsAreValid;
        _writePersistentLoginToCookie = writePersistentLoginToCookie;
        _sessionUser = sessionUser;
    }

    public ActionResult Welcome()
    {
        ViewData["Message"] = "Sind Sie sicher?";

        var model = new WelcomeModel {};

        return View(model);
    }

    public ActionResult Register(){ return View(new RegisterModel()); }
        
    [HttpPost]
    public ActionResult Register(RegisterModel model){

        if (ModelState.IsValid)
        {
            _registerUser.Run(RegisterModelToUser.Run(model));
            return RedirectToAction(Links.RegisterSuccess, Links.WelcomeController);
        }
                
        return View(model);
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


}