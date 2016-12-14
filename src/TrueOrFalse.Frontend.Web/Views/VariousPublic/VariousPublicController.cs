using System.Web.Mvc;

public class VariousPublicController : Controller
{
    public ActionResult Imprint()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult MemuchoBeta()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult AboutMemucho()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult Jobs()
    {
        return View(new BaseModel());
    }

    public ActionResult TermsAndConditions()
    {
        return View(new BaseModel());
    }

    public ActionResult NotDoneYet()
    {
        return View(new BaseModel());
    }

    public ActionResult WelfareCompany()
    {
        return View(new BaseModel());
    }

    public ActionResult Qunit()
    {
        return View(new BaseModel());
    }
}

