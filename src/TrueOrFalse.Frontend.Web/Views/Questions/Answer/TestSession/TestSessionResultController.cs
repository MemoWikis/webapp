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

        if (sessionUser.TestSessions.Count(s => s.Id == testSessionId) > 1)
            throw new Exception($"TestSessionId is not unique, there are {sessionUser.TestSessions.Count(s => s.Id == testSessionId)} test sessions");

        if (sessionUser.TestSessions.Count(s => s.Id == testSessionId) == 0)
            return new TestSession {SessionNotFound = true};

        return sessionUser.TestSessions.Find(s => s.Id == testSessionId);
    }

}