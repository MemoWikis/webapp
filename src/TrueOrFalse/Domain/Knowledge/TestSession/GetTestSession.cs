using System;
using System.Linq;

public class GetTestSession
{
    public static TestSession Get(int testSessionId)
    {
        var sessionUser = Sl.SessionUser;
        if (sessionUser.TestSessions.Count(s => s.Id == testSessionId) > 1)
            throw new Exception($"TestSessionId is not unique, there are {sessionUser.TestSessions.Count(s => s.Id == testSessionId)} test sessions");

        if (sessionUser.TestSessions.Count(s => s.Id == testSessionId) == 0)
            return new TestSession { SessionNotFound = true };

        return sessionUser.TestSessions.Find(s => s.Id == testSessionId);
    }
}
