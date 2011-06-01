using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.Frontend.Web.Controllers
{
    [HandleError]
    public class WelcomeController : Controller
    {
        private readonly RegisterUser _registerUser;
        private readonly CredentialsAreValid _credentialsAreValid;
        private readonly SessionUser _sessionUser;

        public WelcomeController(RegisterUser registerUser, 
                                 CredentialsAreValid credentialsAreValid, 
                                 SessionUser sessionUser)
        {
            _registerUser = registerUser;
            _credentialsAreValid = credentialsAreValid;
            _sessionUser = sessionUser;
        }

        public ActionResult Welcome()
        {
            ViewData["Message"] = "Sind Sie sicher?";

            var model = new WelcomeModel {MostPopular = QuestionDemoData.All()};

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
            loginModel.UserName = Request["UserName"];
            loginModel.Password = Request["Password"];

            if (_credentialsAreValid.Yes(loginModel.UserName, loginModel.Password))
            {
                _sessionUser.Login(_credentialsAreValid.User);
                return RedirectToAction(Links.Summary, Links.SummaryController );
            }

            loginModel.SetToWrongCredentials();

            return View(loginModel);
        }
            
    }
}
