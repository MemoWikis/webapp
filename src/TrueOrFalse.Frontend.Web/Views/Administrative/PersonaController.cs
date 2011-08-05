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

    public ActionResult Robert()
    {
        return LoginUser("Robert");
    }

    public ActionResult Jule()
    {
        return LoginUser("Jule");
    }

    private ActionResult LoginUser(string userName)
    {
        SessionFactory.BuildSchema();
        _sampleData.CreateUsers();
        _sampleData.ImportQuestions(Server.MapPath("~/SampleData/Questions.xml"));
        var robertM = _userRepository.GetByUserName(userName);
        _sessionUser.Login(robertM);

        if (Request["target-url"] != null)
            return Redirect(Request["target-url"]);

        return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
    }
}