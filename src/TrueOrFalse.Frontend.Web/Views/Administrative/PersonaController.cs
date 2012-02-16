using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;
using TrueOrFalse.Frontend.Web.Code;


[AccessOnlyLocal]
public class PersonaController : Controller
{
    private readonly SessionUser _sessionUser;
    private readonly SampleData _sampleData;
    private readonly UserRepository _userRepository;

    public PersonaController(SessionUser sessionUser,
                                SampleData sampleData, 
                                UserRepository userRepository)
    {
        _sessionUser = sessionUser;
        _sampleData = sampleData;
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
        SessionFactory.BuildSchema();
        _sampleData.CreateUsers();
        _sampleData.Import(Server.MapPath("~/SampleData/Export.xml"));
        var robertM = _userRepository.GetByEmailAddress(emailAddress);
        _sessionUser.Login(robertM);

        if (Request["target-url"] != null)
            return Redirect(Request["target-url"]);

        return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
    }
}