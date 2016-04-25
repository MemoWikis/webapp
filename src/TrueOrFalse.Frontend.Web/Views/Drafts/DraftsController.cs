using System.Web.Mvc;

public class DraftsController : BaseController
{
    [AccessOnlyAsAdmin]
    public ActionResult Bootstrap()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Boxes()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult FontAwesome()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Forms()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Grid()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Icons()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult ContentUnits()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult RangeSlider()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Reference()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult temp()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Templates()
    {
        return View(new WelcomeModel());
    }
}