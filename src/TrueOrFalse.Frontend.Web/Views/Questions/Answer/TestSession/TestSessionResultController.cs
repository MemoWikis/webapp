using System;
using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    public const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";
    public const string _viewlocationAsync = "~/Views/Questions/Answer/TestSession/TestSessionResultDetails.ascx";

    [SetThemeMenu(isTestSessionPage: true)]
    public ActionResult TestSessionResult(string name, int testSessionId) => 
        View(_viewLocation, new TestSessionResultModel(GetTestSession.Get(testSessionId)));

   
    public ActionResult TestSessionResultAsync( string testSessionIdString)
    {
        var testSessionId = Int32.Parse(testSessionIdString);
        var testSession = GetTestSession.Get(testSessionId);
        var model = new TestSessionResultModel(testSession);
        var test = PartialView(_viewlocationAsync, model);
        return test;

    }
}