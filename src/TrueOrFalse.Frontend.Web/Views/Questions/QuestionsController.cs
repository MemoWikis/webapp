using System.Web.Mvc;

public class QuestionsController : Controller
{
    public ActionResult Questions()
    {
        return View(new QuestionsModel());
    }
}
