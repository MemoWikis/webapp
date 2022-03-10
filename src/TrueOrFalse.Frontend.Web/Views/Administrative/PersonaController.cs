using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[AccessOnlyAsAdmin]
public class PersonaController : BaseController
{
    private readonly UserRepo _userRepo;

    public PersonaController(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public ActionResult Stefan()
    {
        return LoginUser("noackstefan@googlemail.com");
    }

    public ActionResult Robert()
    {
        return LoginUser("robert@robert-m.de");
    }

    public ActionResult Jule()
    {
        return LoginUser("jule@robert-m.de");
    }

    private ActionResult LoginUser(string emailAddress)
    {
        var robertM = _userRepo.GetByEmail(emailAddress);
        SessionUser.Login(robertM);

        if (Request["target-url"] != null)
            return Redirect(Request["target-url"]);

        return Redirect(Request["target-url"]);
        //return RedirectToAction(Links.KnowledgeAction, Links.KnowledgeController);
    }
}