using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Models;

namespace TrueOrFalse.View.Web
{
    [HandleError]
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            return View(model);
        }
    }
}