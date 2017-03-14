using System;
using System.Linq;
using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";

    public ActionResult TestSessionResult(string name, int testSessionId) => 
        View(_viewLocation, new TestSessionResultModel(GetTestSession(testSessionId)));

    public static TestSession GetTestSession(int testSessionId)
    {
        var sessionUser = Sl.SessionUser;

        if (sessionUser.TestSessions.Count(s => s.Id == testSessionId) != 1)
            throw new Exception("TestSessionId is not unique, there are " + sessionUser.TestSessions.Count(s => s.Id == testSessionId) +
                " results (0 means: session is simply not there yet; >1 means: more than 1 TestSession was created simultaneously with same Id)");

        return sessionUser.TestSessions.Find(s => s.Id == testSessionId);
    }

}