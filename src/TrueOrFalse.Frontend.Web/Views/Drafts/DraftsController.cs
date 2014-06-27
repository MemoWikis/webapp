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
    public ActionResult bootstrap()
    {
        return View(new WelcomeModel());
    }

    public ActionResult Boxes()
    {
        return View(new WelcomeModel());
    }

    public ActionResult Forms()
    {
        return View(new WelcomeModel());
    }

    public ActionResult Grid()
    {
        return View(new WelcomeModel());
    }

    public ActionResult Icons()
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

    public ActionResult Templates()
    {
        return View(new WelcomeModel());
    }

}