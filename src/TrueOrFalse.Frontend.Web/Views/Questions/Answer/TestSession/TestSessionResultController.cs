using System;
using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    public const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";
    public const string _viewlocationAsync = "~/Views/Questions/Answer/TestSession/TestSessionResult.ascx";

    [SetThemeMenu(isTestSessionPage: true)]
    public ActionResult TestSessionResult(string name, int testSessionId) => 
        View(_viewLocation, new TestSessionResultModel(GetTestSession.Get(testSessionId)));

   
    public ActionResult TestSessionResultAsync( string testSessionIdString)
    {
        var testSessionId = Int32.Parse(testSessionIdString);
        var testSession = GetTestSession.Get(testSessionId);

        return PartialView(_viewlocationAsync);
        
    }
}