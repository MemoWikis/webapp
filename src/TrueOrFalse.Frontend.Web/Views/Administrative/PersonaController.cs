using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

[AccessOnlyAsAdmin]
public class PersonaController : BaseController
{
    private readonly UserRepository _userRepository;

    public PersonaController(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
        var robertM = _userRepository.GetByEmail(emailAddress);
        _sessionUser.Login(robertM);

        if (Request["target-url"] != null)
            return Redirect(Request["target-url"]);

        return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
    }
}