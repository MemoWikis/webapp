using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Registration;
using TrueOrFalse.Web;
using TrueOrFalse.Web.Context;
using TrueOrFalse.Frontend.Web.Code;

[HandleError]
public class DraftsController : Controller
{
    
    public ActionResult Boxes()
    {
        return View(new WelcomeModel());
    }

}