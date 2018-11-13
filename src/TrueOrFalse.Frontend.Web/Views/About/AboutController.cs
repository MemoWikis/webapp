using System.Web.Mvc;
public class AboutController : Controller
{
    [SetMainMenu(MainMenuEntry.About)]
    public ActionResult AboutMemucho()
    {
        return View(new AboutMemuchoModel());
    }

    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult ForTeachers()
    {
        return View(new ForTeachersModel());
    }

    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult Jobs()
    {
        return View(new BaseModel());
    }

    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult Master()
    {
        return View(new BaseModel());
    }

    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult WelfareCompany()
    {
        return View(new BaseModel());
    }


}