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

    public ActionResult Grid()
    {
        return View(new WelcomeModel());
    }

    public ActionResult ContentUnits()
    {
        return View(new WelcomeModel());
    }

    public ActionResult RangeSlider()
    {
        return View(new WelcomeModel());
    }

    public ActionResult temp()
    {
        return View(new WelcomeModel());
    }

}