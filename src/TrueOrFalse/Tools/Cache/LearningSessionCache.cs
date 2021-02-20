using System.Collections.Concurrent;
using System.Web;

public class LearningSessionCache
{
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new ConcurrentDictionary<string, LearningSession>();

    public static void AddOrUpdate(LearningSession learningSession)
    {
        if (Sl.SessionUser.IsLoggedIn)
        {
            UserCache.LearningSession = learningSession;
        }
        else
        {
            _learningSessions.AddOrUpdate(
                HttpContext.Current.Session.SessionID, 
                learningSession, 
                (a, b) => learningSession
            );
        }
    }

    public static LearningSession GetLearningSession()
    {
        if (Sl.SessionUser.IsLoggedIn)
        {
            return UserCache.LearningSession;
        }
        else
        {
            _learningSessions.TryGetValue(HttpContext.Current.Session.SessionID, out var learningSession);
            return learningSession;
        }
    }
}