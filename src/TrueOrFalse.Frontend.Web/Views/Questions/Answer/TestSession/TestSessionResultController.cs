using System;
using System.Linq;
using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";

    public ActionResult TestSessionResult(string name, int testSessionId)
    {
        if (_sessionUser.TestSessions.Count(s => s.Id == testSessionId) != 1)
            throw new Exception("TestSessionId is not unique, there are " + _sessionUser.TestSessions.Count(s => s.Id == testSessionId) +
                " results (0 means: session is simply not there yet; >1 means: more than 1 TestSession was created simultaneously with same Id)");

        var testSession = _sessionUser.TestSessions.Find(s => s.Id == testSessionId);

        return View(_viewLocation, new TestSessionResultModel(testSession));
    }

}
