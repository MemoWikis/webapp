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
    public ActionResult Welcome(){
        return View(new WelcomeModel());
    }

    public ActionResult Welcome2(){
        return View(new WelcomeModel());
    }
}