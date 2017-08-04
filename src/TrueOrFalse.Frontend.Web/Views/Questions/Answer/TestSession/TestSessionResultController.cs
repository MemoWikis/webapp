using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";

    [SetThemeMenu(isTestSessionPage: true)]
    public ActionResult TestSessionResult(string name, int testSessionId) => 
        View(_viewLocation, new TestSessionResultModel(GetTestSession.Get(testSessionId)));    
}