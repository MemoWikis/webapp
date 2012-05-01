using System.Web.Mvc;
using TrueOrFalse.Core.Web.Context;

public class KnowledgeController : Controller
{
    private readonly SessionUser _sessionUser;

    public KnowledgeController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    public ActionResult Knowledge()
    {
        return View(new KnowledgeModel(_sessionUser));
    }
}
