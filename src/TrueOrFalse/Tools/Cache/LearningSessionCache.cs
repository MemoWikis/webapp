using System.Collections.Concurrent;
using System.Linq;
using System.Web;

public class LearningSessionCache: IRegisterAsInstancePerLifetime
{
    private readonly string _sessionId;
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new();

    public LearningSessionCache(HttpContext httpContext)
    {
        _sessionId = httpContext.Session.SessionID;
    }
    public void AddOrUpdate(LearningSession learningSession)
    {
        _learningSessions.AddOrUpdate(
            HttpContext.Current.Session.SessionID,
            learningSession,
            (a, b) => learningSession
        );
    }

    public  LearningSession TryRemove()
    {

        _learningSessions.TryRemove(_sessionId, out var learningSession);
        return GetLearningSession();
    }

    public  LearningSession GetLearningSession()
    {
        var context = HttpContext.Current.Session.SessionID;

        _learningSessions.TryGetValue(_sessionId, out var learningSession);
        AddOrUpdate(learningSession);
        return learningSession;
    }

    public void EditQuestionInLearningSession(QuestionCacheItem question)
    {
        var learningSession = GetLearningSession();

        foreach (var step in learningSession.Steps)
            if (step.Question.Id == question.Id)
                step.Question = question;
    }

    public int RemoveQuestionFromLearningSession(int sessionIndex, int questionId)
    {
        var learningSession = GetLearningSession();
        learningSession.Steps = learningSession.Steps.Where(s => s.Question.Id != questionId).ToList();

        if (learningSession.Steps.Count > sessionIndex + 1)
            return sessionIndex;

        return learningSession.Steps.Count - 1;
    }
}