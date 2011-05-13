using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.Frontend.Web.Controllers
{
    [HandleError]
    public class WelcomeController : Controller
    {
        private readonly RegisterUser _registerUser;

        public WelcomeController(RegisterUser registerUser)
        {
            _registerUser = registerUser;
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
                return RedirectToAction(Links.RegisterSuccess, Links.WelcomeController);
            }
                

            return View(model);
        }

        public ActionResult RegisterSuccess() { return View(new RegisterSuccessModel()); }
            
    }
}
